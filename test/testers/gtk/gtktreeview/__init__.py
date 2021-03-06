
##############################################################################
# Written by:  Cachen Chen <cachen@novell.com>
# Date:        06/18/2008
#              Application wrapper for gtktreeview.py
#              Used by the treeview-*.py tests
##############################################################################$

'Application wrapper for gtktreeview'

from strongwind import *

from os.path import exists
from sys import path

def launchTreeView(exe=None):
    'Launch gtktreeview with accessibility enabled and return a treeview object.  Log an error and return None if something goes wrong'

    if exe is None:
        # make sure we can find the sample application
        harness_dir = path[0]
        i = harness_dir.rfind("/")
        j = harness_dir[:i].rfind("/")
        uiaqa_path = harness_dir[:j]
        exe = '%s/samples/gtk/gtktreeview.py' % uiaqa_path
        if not exists(exe):
          raise IOError, "Could not find file %s" % exe
  
    args = [exe]

    (app, subproc) = cache.launchApplication(args=args)

    treeview = GtkTreeView(app, subproc)
    cache.addApplication(treeview)

    treeview.gtkTreeViewFrame.app = treeview

    return treeview

# class to represent the application
class GtkTreeView(accessibles.Application):
    def __init__(self, accessible, subproc=None):
        'Get a reference to the Tree View window'
        super(GtkTreeView, self).__init__(accessible, subproc)
        self.findFrame(re.compile('^Tree View'), logName='Gtk Tree View')

