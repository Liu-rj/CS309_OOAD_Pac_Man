import importlib
import threading
import time
import json
import sys
import os
from socket import *
import copy
import numpy as np
import random

DIRS = [[0, 1], [1, 0], [0, -1], [-1, 0]]

USERS = {}
try:
    with open('./user_data.json', 'r', encoding='utf-8') as file:
        USERS = json.load(file)
except FileNotFoundError:
    pass


class PacmanServer(threading.Thread):
    shared_lock = threading.Lock()

    def __init__(self, conn, address):
        super().__init__()
        self.conn = conn
        self.address = address
        self.user_name = '-1'
        self.action_q = []
        self.index = 0

    def FillMaze(self, x, y, maze):
        for dir in DIRS:
            i = x + dir[0]
            j = y + dir[1]
            if (i >= 0 and j >= 0 and i < 31 and j < 28 and maze[i][j] == '0'):
                maze[i][j] = '1'
                self.FillMaze(i, j, maze)
        return maze

    def AdjustMaze(self, Maze):
        maze = copy.deepcopy(Maze)
        afterFill = self.FillMaze(17, 13, maze)
        for i in range(31):
            for j in range(28):
                if afterFill[i][j] == '0':
                    Maze[i][j] = '1'
        Maze[13][11] = '4'
        Maze[13][16] = '5'
        Maze[15][11] = '6'
        Maze[15][16] = '7'
        Maze[17][13] = '8'
        Maze[17][14] = '9'
        ndmaze = np.array(Maze)
        idx = np.where(ndmaze == '0')
        idx = list(zip(idx[0], idx[1]))
        # id = np.random.randint(0, len(idx))
        # pos = idx[id]
        id_list = random.sample(range(0, len(idx)), 23)

        for i in range(3, 23):
            pos = idx[id_list[i]]
            Maze[pos[0]][pos[1]] = '3'
            Maze[pos[0]][27 - pos[1]] = '3'
        # for i in range(31):
        #     for j in range(28):
        #         if Maze[i][j] == '0':
        #             Maze[i][j] = '2'
        # Maze[14][13]='3'
        for i in range(0, 3):
            pos = idx[id_list[i]]
            Maze[pos[0]][pos[1]] = '2'
            Maze[pos[0]][27 - pos[1]] = '2'
        return Maze

    def check_whether_can_run(self, color, board, strong):
        l1 = len(board)
        l2 = len(board[0])
        position = (-1, -1)
        for i in range(len(board)):
            for j in range(len(board[0])):
                if board[i][j] == color:
                    position = (i, j)
        next_position = []
        i = position[0]
        j = position[1]
        if (
                (board[i - 1][j] == 0 or board[i - 1][j] == 3 or board[i - 1][j] == 2 or (
                        strong and board[i - 1][j] >= 4 and board[i - 1][j] <= 7))):
            next_position.append(((i - 1 + l1) % l1, j))
        ii = (i + 1) % l1
        if ((board[ii][j] == 0 or board[ii][j] == 3 or board[ii][j] == 2 or (
                strong and board[ii][j] >= 4 and board[ii][j] <= 7))):
            next_position.append((ii, j))
        if (
                (board[i][j - 1] == 0 or board[i][j - 1] == 3 or board[i][j - 1] == 2 or (
                        strong and board[i][j - 1] >= 4 and board[i][j - 1] <= 7))):
            next_position.append((i, (j - 1 + l2) % l2))
        jj = (j + 1) % l2
        if ((board[i][jj] == 0 or board[i][jj] == 3 or board[i][jj] == 2 or (
                strong and board[i][jj] >= 4 and board[i][jj] <= 7))):
            next_position.append((i, jj))
        return position, next_position

    def find_ghost_path(self, board, tag, strong1, strong2):
        # if the two pacman are both in strong mode, choose any possible direction
        if strong1 and strong2:
            cp, next_pos = self.check_whether_can_run(tag, board, False)
            for (i, j) in next_pos:
                if board[i][j] != 8 and board[i][j] != 9:
                    return cp, (i, j)
            return cp, (-1, -1)
        # else, find the closest path to pacman
        l1 = len(board)
        l2 = len(board[0])
        board = np.array(board)
        current_pos = np.where(board == tag)
        current_pos = list(zip(current_pos[0], current_pos[1]))[0]
        queue = [[]]
        queue[0].append((-1, current_pos))
        visited = set()
        visited.add(current_pos)
        while True:
            flag2 = True
            last_layer = queue[-1]
            queue.append([])
            candidate_length = len(last_layer)
            flag = False
            for c in range(candidate_length):
                pos = last_layer[c][1]
                i = pos[0]
                j = pos[1]
                ii = (i - 1 + l1) % l1
                if (not ((ii, j) in visited)) and (
                        board[ii][j] == 0 or board[ii][j] == 3 or board[ii][j] == 2 or (
                        not strong1 and
                        board[ii][j] == 8) or (
                                not strong2 and
                                board[ii][j] == 9)):
                    flag2 = False
                    queue[-1].append((c, (ii, j)))
                    visited.add((ii, j))
                    if board[ii][j] >= 8:
                        flag = True
                        break
                ii = (i + 1) % l1
                if (not ((ii, j) in visited)) and (
                        board[ii][j] == 0 or board[ii][j] == 3 or board[ii][j] == 2 or (
                        not strong1 and
                        board[i + 1][j] == 8) or (
                                not strong2 and
                                board[ii][j] == 9)):
                    flag2 = False
                    queue[-1].append((c, (ii, j)))
                    visited.add((ii, j))
                    if board[ii][j] >= 8:
                        flag = True
                        break
                jj = (j - 1 + l2) % l2
                if (not ((i, jj) in visited)) and (
                        board[i][jj] == 0 or board[i][jj] == 3 or board[i][jj] == 2 or (
                        not strong1 and
                        board[i][jj] == 8) or (
                                not strong2 and
                                board[i][jj] == 9)):
                    flag2 = False
                    queue[-1].append((c, (i, jj)))
                    visited.add((i, jj))
                    if (board[i][jj] >= 8):
                        flag = True
                        break
                jj = (j + 1) % l2
                if (not ((i, jj) in visited)) and (
                        board[i][jj] == 0 or board[i][jj] == 3 or board[i][jj] == 2 or (
                        not strong1 and
                        board[i][jj] == 8) or (
                                not strong2 and
                                board[i][jj] == 9)):
                    flag2 = False
                    queue[-1].append((c, (i, jj)))
                    visited.add((i, jj))
                    if board[i][jj] >= 8:
                        flag = True
                        break
            if flag:
                break
            if flag2:
                queue.pop()
                break
        depth = len(queue) - 1
        leaf = queue[depth][-1]
        while depth > 1:
            dad = leaf[0]
            depth -= 1
            leaf = queue[depth][dad]
        return current_pos, leaf[1]

    def check_end(self, board):
        new_board = np.array(board)
        idx = np.where(new_board == 3)
        num = len(idx[0])
        if num == 0:
            return True
        else:
            return False

    def check_valid(self, candidates, board, color, strong):
        current_position, correct_next_position = self.check_whether_can_run(color, board, strong)
        if (len(correct_next_position) == 0):
            if (len(candidates) != 0):
                return -1, current_position
            else:
                return -5, current_position
        else:
            if (len(candidates) == 0):
                return -1, current_position
            else:
                next_pos = candidates[-1]
                diff = abs(next_pos[0] - current_position[0]) + abs(next_pos[1] - current_position[1])
                if abs(next_pos[0] - current_position[0]) == 27 and abs(next_pos[1] - current_position[1]) == 0:
                    diff = 1
                elif abs(next_pos[0] - current_position[0]) == 0 and abs(next_pos[1] - current_position[1]) == 27:
                    diff = 1
                if diff != 1 or (board[next_pos[0]][next_pos[1]] == 1 or (
                        not strong and board[next_pos[0]][next_pos[1]] >= 4 and board[next_pos[0]][next_pos[1]] <= 7)):
                    return -1, current_position
                else:
                    return 1, current_position

    def main_run(self, id1, id2, board):
        current_path = os.getcwd()
        script_dir = os.path.join(current_path)
        sys.path.append(script_dir)
        module1 = importlib.import_module('AI_' + id1)
        constructor1 = module1.AI
        player1 = constructor1(8)
        module2 = importlib.import_module('AI_' + id2)
        constructor2 = module2.AI
        player2 = constructor2(9)
        score1 = 0
        score2 = 0
        hp1 = 1
        hp2 = 1
        strong_step1 = -1
        strong_step2 = -1
        now_player = 0
        pretag2 = 0
        pretag3 = 0
        pretag4 = 0
        pretag5 = 0
        round = 0
        while True:
            round += 1
            if round > 360:
                self.action_q.append((str(6) + ' ' + str(score1) + ' ' + str(score2)).encode())
                print(str(6) + ' ' + str(score1) + ' ' + str(score2))
                break
            if now_player == 0:
                new_board = copy.deepcopy(board)
                player1.go(new_board)
                valid, now_pos = self.check_valid(player1.next_position, board, 8, strong_step1 >= 0)
                if valid == 1:
                    strong1 = strong_step1 >= 0
                    pos = player1.next_position[-1]
                    if board[pos[0]][pos[1]] == 3:
                        score1 += 1
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                    elif strong1 and board[pos[0]][pos[1]] == 4:
                        score1 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                        board[13][11] = 4
                        self.action_q.append((str(0) + ' ' + str(13) + ' ' + str(11)).encode())
                    elif board[pos[0]][pos[1]] == 2:
                        strong_step1 = 0
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                    elif strong1 and board[pos[0]][pos[1]] == 5:
                        score1 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                        board[13][16] = 5
                        self.action_q.append((str(1) + ' ' + str(13) + ' ' + str(16)).encode())
                    elif strong1 and board[pos[0]][pos[1]] == 6:
                        score1 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                        board[15][11] = 6
                        self.action_q.append((str(2) + ' ' + str(15) + ' ' + str(11)).encode())
                    elif strong1 and board[pos[0]][pos[1]] == 7:
                        score1 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                        board[15][16] = 7
                        self.action_q.append((str(3) + ' ' + str(15) + ' ' + str(16)).encode())
                    elif board[pos[0]][pos[1]] == 0:
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 8
                    self.action_q.append((str(4) + ' ' + str(pos[0]) + ' ' + str(pos[1])).encode())
                    print(str(4) + ' ' + str(pos[0]) + ' ' + str(pos[1]))
                elif valid == -1:
                    self.action_q.append((str(7) + ' ' + str(4)).encode())
                    print(str(7) + ' ' + str(4))
                    break
                strong_step1 = (strong_step1 + 1) if strong_step1 >= 0 else -1
                strong_step1 = strong_step1 if (strong_step1 < 11) else -1
                if self.check_end(board):
                    self.action_q.append((str(6) + ' ' + str(score1) + ' ' + str(score2)).encode())
                    print(str(6) + ' ' + str(score1) + ' ' + str(score2))
                    break
            if now_player == 1:
                new_board = copy.deepcopy(board)
                player2.go(new_board)
                valid, now_pos = self.check_valid(player2.next_position, board, 9, strong_step2 >= 0)
                if valid == 1:
                    strong2 = strong_step2 >= 0
                    pos = player2.next_position[-1]
                    if board[pos[0]][pos[1]] == 3:
                        score2 += 1
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                    elif strong2 and board[pos[0]][pos[1]] == 4:
                        score2 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                        board[13][11] = 4
                        self.action_q.append((str(0) + ' ' + str(13) + ' ' + str(11)).encode())
                    elif board[pos[0]][pos[1]] == 2:
                        strong_step2 = 0
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                    elif strong2 and board[pos[0]][pos[1]] == 5:
                        score2 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                        board[13][16] = 5
                        self.action_q.append((str(1) + ' ' + str(13) + ' ' + str(16)).encode())
                    elif strong2 and board[pos[0]][pos[1]] == 6:
                        score2 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                        board[15][11] = 6
                        self.action_q.append((str(2) + ' ' + str(15) + ' ' + str(11)).encode())
                    elif strong2 and board[pos[0]][pos[1]] == 7:
                        score2 += 10
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                        board[15][16] = 7
                        self.action_q.append((str(3) + ' ' + str(15) + ' ' + str(16)).encode())
                    elif board[pos[0]][pos[1]] == 0:
                        board[now_pos[0]][now_pos[1]] = 0
                        board[pos[0]][pos[1]] = 9
                    self.action_q.append((str(5) + ' ' + str(pos[0]) + ' ' + str(pos[1])).encode())
                    print(str(5) + ' ' + str(pos[0]) + ' ' + str(pos[1]))
                elif valid == -1:
                    self.action_q.append((str(7) + ' ' + str(5)).encode())
                    print(str(7) + ' ' + str(5))
                    break
                strong_step2 = (strong_step2 + 1) if strong_step2 >= 0 else -1
                strong_step2 = strong_step2 if (strong_step2 < 11) else -1
                if self.check_end(board):
                    self.action_q.append((str(6) + ' ' + str(score1) + ' ' + str(score2)).encode())
                    print(str(6) + ' ' + str(score1) + ' ' + str(score2))
                    break
            if now_player == 2:
                new_board = copy.deepcopy(board)
                pos, next_pos = self.find_ghost_path(new_board, 4, strong_step1 >= 0, strong_step2 >= 0)
                if next_pos[0] != -1:
                    board[pos[0]][pos[1]] = pretag2
                    pretag2 = board[next_pos[0]][next_pos[1]]
                    self.action_q.append((str(0) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1])).encode())
                    print(str(0) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1]))
                    if board[next_pos[0]][next_pos[1]] == 8:
                        hp1 -= 1
                        pretag2 = 0
                        board[17][13] = 8
                        self.action_q.append((str(4) + ' ' + str(17) + ' ' + str(13)).encode())
                        if hp1 == 0:
                            self.action_q.append((str(8) + ' ' + str(4)).encode())
                            print(str(8) + ' ' + str(4))
                            break
                    if board[next_pos[0]][next_pos[1]] == 9:
                        hp2 -= 1
                        pretag2 = 0
                        board[17][14] = 9
                        self.action_q.append((str(5) + ' ' + str(17) + ' ' + str(14)).encode())
                        if hp2 == 0:
                            self.action_q.append((str(8) + ' ' + str(5)).encode())
                            print(str(8) + ' ' + str(5))
                            break
                    board[next_pos[0]][next_pos[1]] = 4
            if now_player == 3:
                new_board = copy.deepcopy(board)
                pos, next_pos = self.find_ghost_path(new_board, 5, strong_step1 >= 0, strong_step2 >= 0)
                if next_pos[0] != -1:
                    board[pos[0]][pos[1]] = pretag3
                    pretag3 = board[next_pos[0]][next_pos[1]]
                    self.action_q.append((str(1) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1])).encode())
                    print(str(1) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1]))
                    if board[next_pos[0]][next_pos[1]] == 8:
                        hp1 -= 1
                        pretag3 = 0
                        board[17][13] = 8
                        self.action_q.append((str(4) + ' ' + str(17) + ' ' + str(13)).encode())
                        if hp1 == 0:
                            self.action_q.append((str(8) + ' ' + str(4)).encode())
                            print(str(8) + ' ' + str(4))
                            break
                    if board[next_pos[0]][next_pos[1]] == 9:
                        hp2 -= 1
                        pretag3 = 0
                        board[17][14] = 9
                        self.action_q.append((str(5) + ' ' + str(17) + ' ' + str(14)).encode())
                        if hp2 == 0:
                            self.action_q.append((str(8) + ' ' + str(5)).encode())
                            print(str(8) + ' ' + str(5))
                            break
                    board[next_pos[0]][next_pos[1]] = 5
            if now_player == 4:
                new_board = copy.deepcopy(board)
                pos, next_pos = self.find_ghost_path(new_board, 6, strong_step1 >= 0, strong_step2 >= 0)
                if next_pos[0] != -1:
                    board[pos[0]][pos[1]] = pretag4
                    pretag4 = board[next_pos[0]][next_pos[1]]
                    self.action_q.append((str(2) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1])).encode())
                    print(str(2) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1]))
                    if board[next_pos[0]][next_pos[1]] == 8:
                        hp1 -= 1
                        pretag4 = 0
                        board[17][13] = 8
                        self.action_q.append((str(4) + ' ' + str(17) + ' ' + str(13)).encode())
                        if hp1 == 0:
                            self.action_q.append((str(8) + ' ' + str(4)).encode())
                            print(str(8) + ' ' + str(4))
                            break
                    if board[next_pos[0]][next_pos[1]] == 9:
                        hp2 -= 1
                        pretag4 = 0
                        board[17][14] = 9
                        self.action_q.append((str(5) + ' ' + str(17) + ' ' + str(14)).encode())
                        if hp2 == 0:
                            self.action_q.append((str(8) + ' ' + str(5)).encode())
                            print(str(8) + ' ' + str(5))
                            break
                    board[next_pos[0]][next_pos[1]] = 6
            if now_player == 5:
                new_board = copy.deepcopy(board)
                pos, next_pos = self.find_ghost_path(new_board, 7, strong_step1 >= 0, strong_step2 >= 0)
                if next_pos[0] != -1:
                    board[pos[0]][pos[1]] = pretag5
                    pretag5 = board[next_pos[0]][next_pos[1]]
                    self.action_q.append((str(3) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1])).encode())
                    print(str(3) + ' ' + str(next_pos[0]) + ' ' + str(next_pos[1]))
                    if board[next_pos[0]][next_pos[1]] == 8:
                        hp1 -= 1
                        pretag5 = 0
                        board[17][13] = 8
                        self.action_q.append((str(4) + ' ' + str(17) + ' ' + str(13)).encode())
                        if hp1 == 0:
                            self.action_q.append((str(8) + ' ' + str(4)).encode())
                            print(str(8) + ' ' + str(4))
                            break
                    if board[next_pos[0]][next_pos[1]] == 9:
                        hp2 -= 1
                        pretag5 = 0
                        board[17][14] = 9
                        self.action_q.append((str(5) + ' ' + str(17) + ' ' + str(14)).encode())
                        if hp2 == 0:
                            self.action_q.append((str(8) + ' ' + str(5)).encode())
                            print(str(8) + ' ' + str(5))
                            break
                    board[next_pos[0]][next_pos[1]] = 7
            now_player = (now_player + 1) % 6

    def gen_maze(self):
        cmd = 'node MazeGenerator.js'
        r = os.popen(cmd)
        text = r.readline()
        chars = list(text)[:-1]
        r.close()
        maze = []
        index = 0
        for i in range(31):
            maze.append([])
            for j in range(28):
                if chars[index] == '|':
                    maze[i].append('1')
                else:
                    maze[i].append('0')
                index += 1
        return self.AdjustMaze(maze)

    def send_maze(self, maze):
        map_string = ''
        for i in range(31):
            for j in range(28):
                map_string += maze[i][j]
                maze[i][j] = int(maze[i][j])
        self.conn.send(map_string.encode())

    def run(self):
        while True:
            try:
                signal = self.conn.recv(2048)
            except:
                continue
            if signal == b'':
                self.conn.close()
                return
            print(signal)
            self.conn.send('y'.encode())
            # Login Username - Pwd
            if signal == b'0':
                data = receive_all(self.conn).decode()
                if not data:
                    self.conn.send('n'.encode())
                    self.conn.close()
                    return
                login_data = data.split(' ')
                user_name = login_data[0]
                pwd = login_data[1]
                if user_name in USERS.keys():
                    true_pwd = USERS[user_name]['pwd']
                    if pwd == true_pwd:
                        self.user_name = user_name
                        self.conn.send('y'.encode())
                    else:
                        self.conn.send('n'.encode())
                        self.conn.close()
                        return
                else:
                    self.conn.send('n'.encode())
                    self.conn.close()
                    return
            # Upload AI Script
            elif signal == b'1':
                data = receive_all(self.conn).decode()
                if not data:
                    self.conn.send('n'.encode())
                    continue
                with open('AI_' + self.user_name + '.py', 'w', encoding='utf-8') as f:
                    f.write(data.replace('\r\n', '\n'))
                    f.close()
            # PlayTo an Opponent with username
            elif signal == b'2':
                data = receive_all(self.conn).decode()
                if not data:
                    self.conn.send('n'.encode())
                    continue
                current_path = os.getcwd()
                filename1 = os.path.join(current_path, 'AI_' + self.user_name + '.py')
                filename2 = os.path.join(current_path, 'AI_' + data + '.py')
                flag1 = os.path.exists(filename1)
                flag2 = os.path.exists(filename2)
                if flag1 and flag2:
                    self.conn.send('y'.encode())
                    maze = self.gen_maze()
                    self.send_maze(maze)
                    run_thread = threading.Thread(target=self.main_run, args=(self.user_name, data, maze))
                    run_thread.start()
                    recv_buf = ''
                    while True:
                        data = receive_all(self.conn).decode()
                        if data == 'y':
                            recv_buf = 'y'
                        if self.index < len(self.action_q) and recv_buf == 'y':
                            self.conn.send(self.action_q[self.index])
                            recv_buf = ''
                            if self.action_q[self.index][0] in (b'6', b'7', b'8'):
                                self.action_q = []
                                self.index = 0
                                break
                            self.index += 1
                else:
                    self.conn.send('n'.encode())
            elif signal == b'3':
                maze = self.gen_maze()
                maze[17][14] = '0'
                self.send_maze(maze)


def receive_all(the_socket, time_limit=1):
    the_socket.setblocking(0)
    total_data = b''
    begin = time.time()
    while True:
        if total_data and time.time() - begin > time_limit:
            break
        elif time.time() - begin > time_limit * 2:
            break
        try:
            data = the_socket.recv(2048)
            if data:
                total_data += data
                begin = time.time()
            else:
                time.sleep(0.1)
        except error:
            pass
    return total_data


def start_listen(ip, port):
    # Create a server socket, bind it to a port and start listening
    listen_sock = socket(AF_INET, SOCK_STREAM)
    listen_sock.bind((ip, port))
    listen_sock.listen(10)
    while True:
        print('Ready to serve...')

        thread_num = len(threading.enumerate())
        print("主线程：线程数量是%d" % thread_num)
        print(str(threading.enumerate()))

        new_sock, address = listen_sock.accept()
        print('Received a connection from:', address)
        PacmanServer(new_sock, address).start()


if __name__ == "__main__":
    start_listen('127.0.0.1', 5000)
