import importlib
import argparse

def check_whether_can_run(color,board):
    position = (-1, -1)
    for i in range(len(board)):
        for j in range(len(board)):
            if board[i][j] == color:
                position = (i, j)
    next_position=[]
    i = position[0]
    j = position[1]
    if (i > 0 and (board[i - 1][j] == 0 or board[i - 1][j] == -1)):
        next_position.append((i - 1, j))
    if (i < len(board) - 1 and (board[i + 1][j] == 0 or board[i + 1][j] == -1)):
        next_position.append((i + 1, j))
    if (j > 0 and (board[i][j - 1] == 0 or board[i][j - 1] == -1)):
        next_position.append((i, j - 1))
    if (j < len(board)-1 and (board[i][j + 1] == 0 or board[i][j + 1] == -1)):
        next_position.append((i, j + 1))
    return position,next_position

def main(id,color,id2):
    #(-1,-1) means incorrect answer,(-5,-5) means that there does not exist possible direction
    import sys
    import os
    import numpy as np
    save_dir = os.getcwd()
    boardDir = os.path.join(save_dir, 'Data')
    save_dir=os.path.join(save_dir,'UserScripts')
    sys.path.append(save_dir)
    module=importlib.import_module("AI_"+str(id))
    constructor=module.AI
    algo = constructor(color)
    boardFile=os.path.join(boardDir,str(id)+"_"+str(id2))
    board=np.fromfile(boardFile,dtype=np.int32).reshape((50,50)).tolist()
    current_position,correct_next_position=check_whether_can_run(color,board)
    algo.go(board)
    if (len(correct_next_position)==0):
        if (len(algo.next_position)!=0):
            print("%d %d"%(-1,-1),end='')
        else:
            print("%d %d"%(-5,-5),end='')
    else:
        if (len(algo.next_position)==0):
            print("%d %d"%(-1,-1),end='')
        else:
            next_pos=algo.next_position[-1]
            diff=abs(next_pos[0]-current_position[0])+abs(next_pos[1]-current_position[1])
            if diff!=1 or (board[next_pos[0]][next_pos[1]]!=0 and board[next_pos[0]][next_pos[1]]!=-1):
                print("%d %d"%(-1,-1),end='')
            else:
                #print("%d %d"%(next_pos[0],next_pos[1]))
                print(str(next_pos[0])+' '+str(next_pos[1]), end='')

if __name__=='__main__':
    parser = argparse.ArgumentParser(
        formatter_class=argparse.ArgumentDefaultsHelpFormatter)
    parser.add_argument(
        '--player',
        type=int,
        help='player id',
        default=11910000)
    parser.add_argument(
        '--color',
        type=int,
        help='the color of player, when it is 10, it means black, when it is 11, it means white',
        default=10
    )
    parser.add_argument(
        '--player2',
        type=int,
        help='player2 id',
        default=11911424
    )
    args=parser.parse_args()
    main(args.player,args.color,args.player2)