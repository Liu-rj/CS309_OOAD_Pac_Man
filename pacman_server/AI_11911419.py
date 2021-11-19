import numpy as np

def judge_possible(board,position):
    l1=len(board)
    l2=len(board[0])
    if board[position[0]][position[1]]==3 or board[position[0]][position[1]]==2:
        up=board[position[0]-1][position[1]]
        down=board[(position[0]+1)%l1][position[1]]
        left=board[position[0]][position[1]-1]
        right=board[position[0]][(position[1]+1)%l2]
        if (up>=4 and up<=7) or (down>=4 and down<=7) or (left>=4 and left<=7) or (right>=4 and right<=7):
            return False
        return True
    else:
        return False

class AI(object):
    def __init__(self,color):
        self.color=color
        self.next_position=[]

    def go(self,board):
        position=(-1,-1)
        l1=len(board)
        l2=len(board[0])
        for i in range(l1):
            for j in range(l2):
                if board[i][j]==self.color:
                    position=(i,j)
        self.next_position=[]
        pos_candidate=[]
        i=position[0]
        j=position[1]
        ii=(i-1+l1)%l1
        if ((board[ii][j]==0 or board[ii][j]==3 or board[ii][j]==2)):
            pos_candidate.append((ii,j))
        ii=(i+1)%l1
        if ((board[ii][j]==0 or board[ii][j]==3 or board[ii][j]==2)):
            pos_candidate.append((ii,j))
        jj=(j-1+l2)%l2
        if ((board[i][jj]==0 or board[i][jj]==3 or board[i][jj]==2)):
            pos_candidate.append((i,jj))
        jj=(j+1)%l2
        if ((board[i][jj]==0 or board[i][jj]==3 or board[i][jj]==2)):
            pos_candidate.append((i,jj))
        for dir in pos_candidate:
            self.next_position.append(dir)
        for dir in pos_candidate:
            if judge_possible(board,dir):
                self.next_position.append(dir)