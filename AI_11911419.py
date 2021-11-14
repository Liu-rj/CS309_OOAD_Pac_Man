import numpy as np

class AI(object):
    def __init__(self,color):
        self.color=color
        self.next_position=[]

    def go(self,board):
        position=(-1,-1)
        for i in range(len(board)):
            for j in range(len(board)):
                if board[i][j]==self.color:
                    position=(i,j)
        self.next_position=[]
        i=position[0]
        j=position[1]
        if (i>0 and (board[i-1][j]==0 or board[i-1][j]==-1)):
            self.next_position.append((i-1,j))
        if (i<len(board)-1 and(board[i+1][j]==0 or board[i+1][j]==-1)):
            self.next_position.append((i+1,j))
        if (j>0 and (board[i][j-1]==0 or board[i][j-1]==-1)):
            self.next_position.append((i,j-1))
        if (j<len(board)-1 and(board[i][j+1]==0 or board[i][j+1]==-1)):
            self.next_position.append((i,j+1))