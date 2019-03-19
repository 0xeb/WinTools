"""
TODO:
------

-- Allow numbering per group. For instance:

/* A (1) */
/* A (2) */
/* B (1) */
/* (1) */
/* (2) */

That means 3 numbering groups in total

-- Allow switch to remove instead of injecting
"""
import os, sys, re, codecs
from optparse import OptionParser

marker_re = re.compile(r"\/\*[^\(]*\(([\d]+|\*)[^\)]*\)\s*\*\/")

#---------------------------------------------------------------------
def parse_args():
    """
    Parses the arguments and returns a triplet
    (bool, options, args)
    The first bool designates whether arguments satisfy the program. If it is false, then the caller should terminate the program
    """
    parser = OptionParser('usage: %prog [options]')

    parser.add_option('-i', '--input', 
                      type="string", 
                      dest='input_file', 
                      help='Input file name', 
                      default=None)

    parser.add_option('-o', '--output', 
                      type="string", 
                      dest='output_file', 
                      help='Output file name', 
                      default=None)

    parser.add_option('-c', '--col', 
                      type="int", 
                      dest='col', 
                      help='Column location', 
                      default=None)

    try:
        (options, args) = parser.parse_args()
    except SystemExit:
        exit(0)
    except:
        print("Error while parsing the arguments. Trying running the tool again with the -h or --help switch")
        return (False, None, None)


    return (True, options, args)


# ----------------------------------------------------------
def main(args):
    try:
        ok, options, args = parse_args()
    except SystemExit:
        exit(0)
    except Exception as e:
        print("Error while parsing the arguments. Trying running the tool again with the -h or --help switch")
        return

    try:
        f = codecs.open(options.input_file, "r", encoding = "utf-8")
    except:
        print("Failed to open input file %s" % args[1])

    lines = f.readlines()
    f.close()

    try:
        f = codecs.open(options.output_file, "w", encoding = "utf-8")
    except:
        print("Failed to create output file for writing: %s" % options.output_file)
        return


    nmatch = 1
    inject_point = options.col
    for line in lines:
        line = line.rstrip('\n').rstrip('\r')
        m = marker_re.search(line)         
        if m is not None:
            line = line[:m.start()] + line[m.end():]
            nline = len(line)
            if nline <= inject_point:
                line = line + ((inject_point - nline) * ' ')
               
            line += ' /* (%d) */' % (nmatch)

            nmatch += 1

        f.write(line + os.linesep)

    f.close()

    print("Success!")

# ----------------------------------------------------------
if __name__ == '__main__':
    main(sys.argv)

