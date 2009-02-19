#!/usr/bin/env python

#######################################################################
# Written by:  Cachen Chen <cachen@novell.com>
# Date:        08/06/2008
# Description: Test accessibility of scrollbar widget 
#              Use the scrollbarframe.py wrapper script
#              Test the samples/checkedlistbox.py script
#######################################################################

# The docstring below  is used in the generated log file
"""
Test accessibility of scrollbar widget
"""

# imports
import sys
import os

from strongwind import *
from scrollbar import *
from helpers import *
from sys import argv
from os import path

app_path = None 
try:
  app_path = argv[1]
except IndexError:
  pass #expected

# open the hscrollbar sample application
try:
  app = launchScrollBar(app_path)
except IOError, msg:
  print "ERROR:  %s" % msg
  exit(2)

# make sure we got the app back
if app is None:
  exit(4)

# just an alias to make things shorter
sbFrame = app.scrollBarFrame

#check vscrollbar's states list
statesCheck(sbFrame.hscrollbar, "HScrollBar")
statesCheck(sbFrame.vscrollbar, "VScrollBar")

sbFrame.mouseClick(log=False)

##scroll the vertical bar, use mouseClick to check if the bar is scrolling, 
##then assert scrollbar value

#set value to 10
sbFrame.valueScrollBar(sbFrame.vscrollbar, 10)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.vscrollbar, 10)

sbFrame.list1item[10].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list1item[10], "ListItem", add_states=["focused", "selected"])

#set value to 29
sbFrame.valueScrollBar(sbFrame.vscrollbar, 29)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.vscrollbar, 29)

sbFrame.list1item[29].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list1item[29],"ListItem", add_states=["focused", "selected"])

#set value to 30, the maximum value is 29
sbFrame.valueScrollBar(sbFrame.vscrollbar, 30)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.vscrollbar, 30)

sbFrame.list1item[29].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list1item[29], "ListItem", add_states=["focused", "selected"])

#set value to 0
sbFrame.valueScrollBar(sbFrame.vscrollbar, 0)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.vscrollbar, 0)

sbFrame.list1item[0].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list1item[0], "ListItem", add_states=["focused", "selected"])

#set value to -10, the minimum value is 0
sbFrame.valueScrollBar(sbFrame.vscrollbar, -10)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.vscrollbar, -10) 

sbFrame.list1item[4].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list1item[4], "ListItem", add_states=["focused", "selected"])

#use keyboard to scroll bar to assert the value
sbFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.vscrollbar, 1) 

##scroll the horizontal bar, use mouseClick to check if the bar is scrolling, 
##then assert scrollbar value

#set value to 3
sbFrame.valueScrollBar(sbFrame.hscrollbar, 3)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.hscrollbar, 3)

sbFrame.mouseClick(log=False)
sbFrame.list2item[15].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list2item[15], "ListItem", add_states=["focused", "selected"])

#set value to 7
sbFrame.valueScrollBar(sbFrame.hscrollbar, 7)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.hscrollbar, 7)

sbFrame.list2item[29].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list2item[29], "ListItem", add_states=["focused", "selected"])

#set value to 8, the maximum value is 7
sbFrame.valueScrollBar(sbFrame.hscrollbar, 8)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.hscrollbar, 8)

sbFrame.list2item[29].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list2item[29], "ListItem", add_states=["focused", "selected"])

#set value to 0
sbFrame.valueScrollBar(sbFrame.hscrollbar, 0)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.hscrollbar, 0) 

sbFrame.list2item[0].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list2item[0], "ListItem", add_states=["focused", "selected"])

#set value to -10, the maximum value is 0
sbFrame.valueScrollBar(sbFrame.hscrollbar, -10)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.hscrollbar, -10)

sbFrame.list2item[4].mouseClick()
sleep(config.SHORT_DELAY)
statesCheck(sbFrame.list2item[4], "ListItem", add_states=["focused", "selected"])

#use keyboard to scroll bar to assert the value
sbFrame.keyCombo("Left", grabFocus=False)
sleep(config.SHORT_DELAY)
sbFrame.assertScrollbar(sbFrame.hscrollbar, 0) 

print "INFO:  Log written to: %s" % config.OUTPUT_DIR

#close application frame window
sbFrame.quit()
