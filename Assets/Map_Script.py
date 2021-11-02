class Map_Script(object):
    def __init__(self, width, height):
        self.width = width
        self.height = height
        self.map = [[1] * width for i in range(height)]

    def map_generation(self):
        # write your code here
        # self.map = [[0] * self.width for i in range(20)]
        # c = [[-1] * self.width for i in range(10)]
        # b = [[1] * self.width for i in range(20)]
        # self.map = self.map + c + b
        b=[[0]*self.width for i in range(1)]
        self.map=[[0]*self.width for i in range(1)]
        a=[[0,1,1,1,1,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1],
           [0,1,1,0,0,0,0,1,1,1,0,0,1,1,0,0,0,0,0,0,0,0,-1,0,0,0,-1,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,-1,0,0,0,0,0,1],
           [0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,-1,0,0,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,0,0],
           [0,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,0,0,0,0,0,0]]
        for i in range(12):
            self.map+=a
        self.map+=b
        return self.map