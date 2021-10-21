import tkinter.filedialog
from shutil import copy
import importlib
import argparse
def main(id,width,height):
    import sys
    import os
    file = tkinter.filedialog.askopenfilename()
    save_dir = os.getcwd()
    save_dir=os.path.join(save_dir,'UserScripts')
    if not (os.path.exists(save_dir)):
        os.makedirs(save_dir)
    sys.path.append(save_dir)
    file_path, file_name = os.path.split(file)
    copy(file, save_dir)
    save_file = os.path.join(save_dir, str(id)+'.py')
    if os.path.isfile(save_file):
        os.remove(save_file)
    os.rename(os.path.join(save_dir, file_name), save_file)
    module=importlib.import_module(str(id))
    constructor=module.Map_Script
    algo = constructor(width,height)
    map=algo.map_generation()
    for i in range(height-1):
        flag=True
        for j in range(width):
            if not flag:
                print(" "+str(map[i][j]),end='')
            else:
                flag=False
                print(map[i][j],end='')
        print(',',end='')
    flag=True
    for j in range(width):
        if not flag:
            print(" "+str(map[height-1][j]),end='')
        else:
            flag=False
            print(map[height-1][j],end='')
    

if __name__=='__main__':
    parser = argparse.ArgumentParser(
        formatter_class=argparse.ArgumentDefaultsHelpFormatter)
    parser.add_argument(
        '--id',
        type=int,
        help='user id',
        default=11910000)
    parser.add_argument(
        '--width',
        type=int,
        help='the width of maze',
        default=8
    )
    parser.add_argument(
        '--height',
        type=int,
        help='the height of maze',
        default=8
    )
    args=parser.parse_args()
    main(args.id,args.width,args.height)