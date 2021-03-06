#!/usr/bin/env python

##############################################################################
# Written by:  Brian G. Merrell <bgmerrell@novell.com>
# Date:        02/12/2009
# Description: Test accessibility of treeview widget 
#              Use the treeviewframe.py wrapper script
#              Test the samples/winforms/treeview.py script
##############################################################################

# The docstring below  is used in the generated log file
"""
Test accessibility of treeview widget
"""

# imports
import sys
import os

from strongwind import *
from treeview import *
from helpers import *
from states import *
from actions import *
from sys import argv
from os import path

app_path = None 
try:
  app_path = argv[1]
except IndexError:
  pass #expected

# open the label sample application
try:
  app = launchTreeView(app_path)
except IOError, msg:
  print "ERROR:  %s" % msg
  exit(2)

# just an alias to make things shorter
tvFrame = app.treeViewFrame

# check the states of all the accessibles
statesCheck(tvFrame.tree_table, "TreeTable", add_states=[FOCUSED])
statesCheck(tvFrame.parent1,
            "TreeViewTableCell",
            add_states=[EXPANDABLE, FOCUSED, SELECTED])
statesCheck(tvFrame.parent2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE])
statesCheck(tvFrame.child1,
            "TreeViewTableCell",
            invalid_states=[SHOWING, VISIBLE, FOCUSABLE])
statesCheck(tvFrame.child2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE],
            invalid_states=[SHOWING, VISIBLE, FOCUSABLE])
statesCheck(tvFrame.child3,
            "TreeViewTableCell",
            invalid_states=[SHOWING, VISIBLE, FOCUSABLE])
statesCheck(tvFrame.grandchild,
            "TreeViewTableCell",
            add_states=[EXPANDABLE],
            invalid_states=[SHOWING, VISIBLE, FOCUSABLE])
statesCheck(tvFrame.great_grandchild,
            "TreeViewTableCell",
            invalid_states=[SHOWING, VISIBLE, FOCUSABLE])

# check the actions of all the accessibles
actionsCheck(tvFrame.parent1,
             "TreeViewTableCell",
             add_actions=[EXPAND_OR_CONTRACT])
actionsCheck(tvFrame.parent2,
             "TreeViewTableCell",
             add_actions=[EXPAND_OR_CONTRACT])
actionsCheck(tvFrame.child1,
             "TreeViewTableCell")
# child2 has a child
actionsCheck(tvFrame.child2,
             "TreeViewTableCell",
             add_actions=[EXPAND_OR_CONTRACT])
actionsCheck(tvFrame.child3,
             "TreeViewTableCell")
actionsCheck(tvFrame.grandchild,
             "TreeViewTableCell",
             add_actions=[EXPAND_OR_CONTRACT])
actionsCheck(tvFrame.great_grandchild,
             "TreeViewTableCell")

# move down using the keyboard and make sure the states change as expected
tvFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
statesCheck(tvFrame.parent1,
            "TreeViewTableCell",
            add_states=[EXPANDABLE])
statesCheck(tvFrame.parent2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE, FOCUSED, SELECTED])
# move back up and expand parent 1, then check to make sure states have changed
# as expected
tvFrame.keyCombo("Up", grabFocus=False)
sleep(config.SHORT_DELAY)
tvFrame.keyCombo("Right", grabFocus=False)
sleep(config.SHORT_DELAY)
statesCheck(tvFrame.parent1,
            "TreeViewTableCell",
            add_states=[EXPANDABLE, EXPANDED, FOCUSED, SELECTED])
statesCheck(tvFrame.parent2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE])
tvFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
statesCheck(tvFrame.child1,
            "TreeViewTableCell",
            add_states=[FOCUSED, SELECTED])
statesCheck(tvFrame.child2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE])
# expand child 2
tvFrame.child2.expandOrContract()
sleep(config.SHORT_DELAY)
statesCheck(tvFrame.child1,
            "TreeViewTableCell",
            add_states=[FOCUSED, SELECTED])
statesCheck(tvFrame.child2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE, EXPANDED])
# contract parent 1
tvFrame.parent1.expandOrContract()
sleep(config.SHORT_DELAY)
statesCheck(tvFrame.child1,
            "TreeViewTableCell",
            invalid_states=[SHOWING, VISIBLE, FOCUSABLE])
statesCheck(tvFrame.parent1,
            "TreeViewTableCell",
            add_states=[FOCUSED, SELECTED, EXPANDABLE])
statesCheck(tvFrame.child2,
            "TreeViewTableCell",
            add_states=[EXPANDABLE, EXPANDED],
            invalid_states=[SHOWING, VISIBLE,FOCUSABLE ])	

#close application frame window
tvFrame.quit()
