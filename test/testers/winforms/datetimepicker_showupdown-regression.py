#!/usr/bin/env python
# vim: set tabstop=4 shiftwidth=4 expandtab
##############################################################################
# Written by:  Ray Wang <rawang@novell.com>
# Date:        01/13/2008
# Description: main test script of datetimepicker_showupdown
#              ../samples/winforms/datetimepicker_showupdown.py is the test sample script
#              datetimepicker_showupdown/* are the wrappers of datetimepicker_showupdown test sample 
##############################################################################

# WONTFIX506159 - DateTimePicker spin button accessibles erroneously
# implemented the EditableText interface

# The docstring below is used in the generated log file
"""
Test accessibility of DateTimePicker widget
"""

# imports
from datetimepicker_showupdown import *
from helpers import *
from actions import *
from states import *
from sys import argv
import datetime

app_path = None 
try:
    app_path = argv[1]
except IndexError:
    pass #expected

# open the datetimepicker_showupdown sample application
try:
    app = launchDateTimePicker(app_path)
except IOError, msg:
    print "ERROR:  %s" % msg
    exit(2)

# make sure we got the app back
if app is None:
    exit(4)


# just an alias to make things shorter
dtpsudFrame = app.dateTimePickerShowUpDownFrame

#############################
# check datetimepicker_showupdown  AtkAction
#############################
actionsCheck(dtpsudFrame.checkbox, "CheckBox")
for i in range(7):
    actionsCheck(dtpsudFrame.weekdays[i], "TableCell")
for i in range(12):
    actionsCheck(dtpsudFrame.months[i], "TableCell")

##############################
# check datetimepicker_showupdown AtkAccessible
##############################
# BUG506082 DateTimePicker panel accessible should not have "focusable" and
# "focused" states
#statesCheck(dtpsudFrame.panel, "Panel", add_states=["focused"])
statesCheck(dtpsudFrame.weekday, "TreeTable")

NUM_MONTHS = 12
NUM_DAYS = 7
# subtract 1 because so January is 0 and December is 11 so we can iterate 
# through range(NUM_MONTHS)
CURRENT_MONTH = dtpsudFrame.localtime[1] - 1
CURRENT_WEEKDAY = dtpsudFrame.localtime[6]
CURRENT_YEAR = dtpsudFrame.localtime[0]
CURRENT_DAY = dtpsudFrame.localtime[2]

# check the status of all days
for i in range(NUM_DAYS):
    if i == CURRENT_WEEKDAY:
        add_states=["focused", "selected"]
        invalid_states=[]
    else:
        add_states=[]
        invalid_states=["showing", "visible"]
    statesCheck(dtpsudFrame.weekdays[i], "TableCell", invalid_states, add_states)

statesCheck(dtpsudFrame.commas[0], "Label")
statesCheck(dtpsudFrame.spaces[0], "Label")
statesCheck(dtpsudFrame.month, "TreeTable")

# check the status of all months
for i in range(NUM_MONTHS):
    if i == CURRENT_MONTH:
        add_states=["focused", "selected"]
        invalid_states=[]
    else:
        add_states=[]
        invalid_states=["showing", "visible"]
    statesCheck(dtpsudFrame.months[i], "TableCell", invalid_states, add_states)

statesCheck(dtpsudFrame.spaces[1], "Label")
statesCheck(dtpsudFrame.day, "DateTimePicker_Spin")
statesCheck(dtpsudFrame.commas[1], "Label")
statesCheck(dtpsudFrame.spaces[2], "Label")
statesCheck(dtpsudFrame.year, "DateTimePicker_Spin")
statesCheck(dtpsudFrame.checkbox, "CheckBox", add_states=["checked", "focused"])
sleep(config.SHORT_DELAY)

dtpsudFrame.assertUneditableText(dtpsudFrame.spaces[0], "test")
dtpsudFrame.assertText(dtpsudFrame.spaces[0], " ")
dtpsudFrame.assertUneditableText(dtpsudFrame.commas[0], "test")
dtpsudFrame.assertText(dtpsudFrame.commas[0], ",")
dtpsudFrame.assertUneditableText(dtpsudFrame.checkbox, "test")
dtpsudFrame.assertText(dtpsudFrame.checkbox, "")

# BUG506252 DateTimePicker table cell accessibles erroneously implement the
# EditableText interface
#dtpsudFrame.assertUneditableText(dtpsudFrame.months[0], "test")
#dtpsudFrame.assertText(dtpsudFrame.months[0], "January")
#dtpsudFrame.assertUneditableText(dtpsudFrame.weekdays[0], "test")
#dtpsudFrame.assertText(dtpsudFrame.weekdays[0], "Monday")

##############################
# Value
##############################
# change the value of the day of the month and year spin buttons, then make
# sure that the GUI updated appropriately.
dtpsudFrame.assignValue(dtpsudFrame.day, 20)
mkdate = datetime.date(CURRENT_YEAR, CURRENT_MONTH + 1, 20)
DATE = mkdate.strftime("%A, %B %d, %Y")
sleep(config.SHORT_DELAY)
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

dtpsudFrame.assignValue(dtpsudFrame.year, 2008)
mkdate = datetime.date(2008, CURRENT_MONTH + 1, 20)
DATE = mkdate.strftime("%A, %B %d, %Y")
sleep(config.SHORT_DELAY)
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

##############################
# Selection
##############################
# we make the value of DateTimePicker to 2009-1-1
dtpsudFrame.selectChild(dtpsudFrame.month, 0)
sleep(config.SHORT_DELAY)

dtpsudFrame.assignValue(dtpsudFrame.day, 1)
sleep(config.SHORT_DELAY)

dtpsudFrame.assignValue(dtpsudFrame.year, 2009)
sleep(config.SHORT_DELAY)

# then we set weekday to Tuesday for instance, 
# the date will be Tuesday, December 30, 2008
dtpsudFrame.selectChild(dtpsudFrame.weekday, 1)
sleep(config.SHORT_DELAY)

mkdate = datetime.date(2008, 12, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
sleep(config.SHORT_DELAY)
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

##############################
# checkbox is disabled (so the date should not change when we try to change it)
##############################
dtpsudFrame.click(dtpsudFrame.checkbox)
sleep(config.SHORT_DELAY)

PREVIOUS_DATE = DATE
dtpsudFrame.assignValue(dtpsudFrame.day, 02)
sleep(config.SHORT_DELAY)
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % PREVIOUS_DATE)

dtpsudFrame.assignValue(dtpsudFrame.year, 1980)
sleep(config.SHORT_DELAY)
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % PREVIOUS_DATE)

dtpsudFrame.selectChild(dtpsudFrame.month, 0)
sleep(config.SHORT_DELAY)
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % PREVIOUS_DATE)

##############################
# test Up/Down 
##############################
# make checkbox checked
dtpsudFrame.keyCombo("space", grabFocus=False)
# test Up/Down on weekday field
dtpsudFrame.keyCombo("Right", grabFocus=False)
sleep(config.SHORT_DELAY)
dtpsudFrame.keyCombo("Up", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2008, 12, 31)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

dtpsudFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2008, 12, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

# test Up/Down on month field
dtpsudFrame.keyCombo("Right", grabFocus=False)
sleep(config.SHORT_DELAY)
dtpsudFrame.keyCombo("Up", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2009, 1, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

dtpsudFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2008, 12, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

# test Up/Down on day field
dtpsudFrame.keyCombo("Right", grabFocus=False)
sleep(config.SHORT_DELAY)
dtpsudFrame.keyCombo("Up", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2008, 12, 31)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

dtpsudFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2008, 12, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

# test Up/Down on year field
dtpsudFrame.keyCombo("Right", grabFocus=False)
sleep(config.SHORT_DELAY)
dtpsudFrame.keyCombo("Up", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2009, 12, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

dtpsudFrame.keyCombo("Down", grabFocus=False)
sleep(config.SHORT_DELAY)
mkdate = datetime.date(2008, 12, 30)
DATE = mkdate.strftime("%A, %B %d, %Y")
dtpsudFrame.assertText(dtpsudFrame.label, "The date you select is: %s" % DATE)

##############################
# End
##############################
# close application frame window
dtpsudFrame.quit()

print "INFO:  Log written to: %s" % config.OUTPUT_DIR
