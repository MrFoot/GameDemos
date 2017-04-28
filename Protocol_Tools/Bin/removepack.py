#!/usr/bin/python
# -*- coding: utf-8 -*-
#
import os, sys, re

Packagelist = []

def DoPackage(file, input, output):
    global Packagelist
    filename = os.path.join(input, file)
    outname = os.path.join(output, file)
    fileopen = open(filename, 'rb').read()
    for package in Packagelist:
        packstr = package + '.'
        fileopen=fileopen.replace(packstr,"")
    #newfilename=filename.replace(".proto","_new.proto")
    open(outname, 'wb').write(fileopen)
    return

def GetPackage(filename):
    global Packagelist
    fp = open(filename, 'rb')
    alllines=fp.readlines();
    fp.close()
    for eachline in alllines:
        if (re.match("package", eachline)):
            temp=eachline.replace("package","");
            temp=temp.replace(" ","");
            temp=temp.replace(";","");
            temp=temp.replace("\n","");
            temp=temp.replace("\r","");
            Packagelist.append(temp);
            return
    return
    

def readproto(dirpath, outdir):
    if not os.path.isdir(dirpath):
        print '%s is not directory' % dirpath
        return
    if not os.path.isdir(outdir):
        print '%s is not outdir' % outdir
        return
    input = os.path.abspath(dirpath)
    output = os.path.abspath(outdir)
    os.chdir(input)
    for parent,dirnames,filenames in os.walk('.'):
        for file in filenames:
            if (file.find('.proto') < 0) or (file.find('.svn') >= 0):
                continue
            src = os.path.join(input, file)
            if os.path.isfile(src):
                GetPackage(src)
                
    #处理文件
    for parent,dirnames,filenames in os.walk('.'):
        for file in filenames:
            if (file.find('.proto') < 0) or (file.find('.svn') >= 0):
                continue
            src = os.path.join(input, file)
            if os.path.isfile(src):
                DoPackage(file, input, output)
    return

def main():
    if len(sys.argv) < 2:
        print 'Usage: <input_dir>' 
        sys.exit(1)
    readproto(sys.argv[1], sys.argv[2])
    global Packagelist
    print Packagelist
    

if __name__ == '__main__':
    main()
