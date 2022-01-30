from shutil import copy
import importlib
import argparse

def checkValidation(board):
    startPosition=(0,0)
    four_ghost=[0,0,0,0]
    width=len(board)
    if width<=0:
        #print('width')
        return [[-2]]
    height=len(board[0])
    for i in range(width):
        for j in range(height):
            if board[i][j]==4 or board[i][j]==5 or board[i][j]==6 or board[i][j]==7:
                four_ghost[board[i][j]-4]+=1
    for i in range(4):
        if four_ghost[i]!=1:
            return [[-2]]
    for i in range(width):
        if (board[i][0]!=1) or (board[i][height-1]!=1):
            #print('wall 1')
            return [[-2]]
    for j in range(height):
        if board[0][j]!=1 or board[width-1][j]!=1:
            #print('wall 2')
            return [[-2]]
    numOfPac=0
    for i in range(width):
        for j in range(height):
            if board[i][j]==8 or board[i][j]==9:
                startPosition=(i,j)
                numOfPac+=1
    if numOfPac!=1:
        #print('wrong pacman')
        return [[-2]]
    visitedMap=[]
    for i in range(width):
        visitedMap.append([])
        for j in range(height):
            visitedMap[i].append(board[i][j])
    queue=[startPosition]
    visitedMap[0][0]=1
    while len(queue)>0:
        pos=queue.pop(0)
        if (pos[0]>0 and visitedMap[pos[0]-1][pos[1]]!=1):
            visitedMap[pos[0]-1][pos[1]]=1
            queue.append((pos[0]-1,pos[1]))
        if (pos[1]>0 and visitedMap[pos[0]][pos[1]-1]!=1):
            visitedMap[pos[0]][pos[1]-1]=1
            queue.append((pos[0],pos[1]-1))
        if (pos[0]<width-1 and visitedMap[pos[0]+1][pos[1]]!=1):
            visitedMap[pos[0]+1][pos[1]]=1
            queue.append((pos[0]+1,pos[1]))
        if (pos[1]<height-1 and visitedMap[pos[0]][pos[1]+1]!=1):
            visitedMap[pos[0]][pos[1]+1]=1
            queue.append((pos[0],pos[1]+1))
    flag=True
    for i in range(width):
        for j in range(height):
            if (visitedMap[i][j]!=1):
                #print('bu lian tong')
                flag=False
                break
    if not flag:
        board[0][0]=-2
    return board

def main(file):
    import sys
    import os
    file_path, file_name = os.path.split(file)
    file_name=file_name.split(".")[0]
    sys.path.append(file_path)
    module=importlib.import_module(file_name)
    constructor=module.Map_Script
    algo = constructor()
    map=algo.map_generation()
    map=checkValidation(map)
    width=len(map)
    height=len(map[0])
    for i in range(width):
        for j in range(height):
            print(str(map[i][j]),end='')
    print(' '+str(width)+' '+str(height))
    

if __name__=='__main__':
    parser = argparse.ArgumentParser(
        formatter_class=argparse.ArgumentDefaultsHelpFormatter)
    parser.add_argument(
        '--file',
        type=str,
        help='file')
    args=parser.parse_args()
    main(args.file)