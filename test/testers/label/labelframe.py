
##############################################################################
# Written by:  Cachen Chen <cachen@novell.com>
# Date:        08/07/2008
# Description: label.py wrapper script
#              Used by the label-*.py tests
##############################################################################

import sys
import os
import actions
import states

from strongwind import *
from label import *


# class to represent the main window.
class LabelFrame(accessibles.Frame):

    # constants
    # the available widgets on the window
    BUTTON = "button2"
    SENSTIVE_LABEL = "there is nothing now."
    INSENSITIVE_LABEL = "I'm so insensitive"

    def __init__(self, accessible):
        super(LabelFrame, self).__init__(accessible)
        self.button = self.findPushButton(self.BUTTON)
        self.sensitive_label = self.findLabel(self.SENSTIVE_LABEL)
        self.insensitive_label = self.findLabel(self.INSENSITIVE_LABEL)

    #give 'click' action
    def click(self,button):
        button.click()

    #check the Label text after click button2
    def assertLabel(self, labelText):
        procedurelogger.expectedResult('Label text has been changed to "%s"' % labelText)
        self.findLabel(labelText)

    #check label's text value
    def assertText(self, textValue):
        #initialize label again to get the new text value
        procedurelogger.expectedResult('Label\'s text value shows in accerciser is "%s"' % textValue)
        def resultMatches():
            return self.sensitive_label.text == textValue
        assert retryUntilTrue(resultMatches)
    
    #close application main window after running test
    def quit(self):
        self.altF4()
