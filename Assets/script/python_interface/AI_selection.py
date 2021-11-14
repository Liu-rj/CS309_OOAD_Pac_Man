import tkinter.filedialog
from shutil import copy
import argparse


def main(id):
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
    save_file = os.path.join(save_dir, 'AI_'+str(id)+'.py')
    if os.path.isfile(save_file):
        os.remove(save_file)
    os.rename(os.path.join(save_dir, file_name), save_file)
    print("1")
    

if __name__=='__main__':
    parser = argparse.ArgumentParser(
        formatter_class=argparse.ArgumentDefaultsHelpFormatter)
    parser.add_argument(
        '--player',
        type=int,
        help='player id',
        default=11910000)
    args=parser.parse_args()
    main(args.player)