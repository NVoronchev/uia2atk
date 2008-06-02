# -*- coding: utf-8 -*-

##############################################################################$
# Written by:  Brian G. Merrell <bgmerrell@novell.com>$
# Date:        May 23 2008$
# Description: checkButton.py wrapper script
#              Used by the checkbutton-*.py tests
##############################################################################$

from strongwind import *
from checkbutton import *


# class to represent the main window.
class CheckbuttonFrame(accessibles.Frame):

    # constants
    # the available widgets on the window
    CHECK_BUTTON_ONE = "check button 1"
    CHECK_BUTTON_TWO = "check button 2"
    BUTTON_QUIT = "Quit"

    # available results for the check boxes
    RESULT_UNCHECKED = "unchecked"
    RESULT_CHECKED = "checked"
    # end constants

    logName = 'CheckButton'

    def __init__(self, accessible):
        super(CheckbuttonFrame, self).__init__(accessible)
        self.checkbox1 = self.findCheckBox(self.CHECK_BUTTON_ONE)
        self.checkbox2 = self.findCheckBox(self.CHECK_BUTTON_TWO)

    def assertResult(self, checkbutton, result):
        'Raise exception if the checkbutton does not match the given result'   
        procedurelogger.expectedResult('%s is %s.' % (checkbutton, result))

        # XXX: Check the result programatically, see the gcalctool example


    def quit(self):
        'Quit checkbutton'

	# click the quit button
	self.app.findPushButton(self.BUTTON_QUIT).click()

        self.assertClosed()

    def assertClosed(self):
        super(CheckbuttonFrame, self).assertClosed()

        # if the checkbutton window closes, the entire app should close.  
        # assert that this is true 
        self.app.assertClosed()
