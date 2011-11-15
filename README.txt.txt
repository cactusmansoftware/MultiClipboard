Welcome to MultiClipboard.
Ok, that was a pretty cheesy intro, but I couldn't really think of anything else to say.
Anyway, this is an application I came up with to solve a problem I was having in my regular job.
I often found myself having to copy/paste a few strings many different times, and it was a huge waste
of time to have to re-copy each string everytime.  So I created MultiClipboard as a way to map up to 10 strings
to each of the number keys across the top of your keyboard (NOT the keys on the numpad.  Yes, there's a difference,
or at least your operating system thinks there is!).

To use MultiClipboard, you can just run one of the exes under the bin/Debug or bin/Release folders.  The app will
start out as a notify icon in your system tray as I meant for it to be as unobtrusive as possible.  By default,
whatever is currently in your Clipboard will be mapped to the 1 key.  To switch between keys, use Ctrl + <number>.
So for example, let's say you load up MultiClipboard with 'abcd' currently in your clipboard.  To store another string
mapped to your 2 key, you would press Ctrl+2, select your text (say, 'efgh') and do Ctrl+C like normal.  Now 'abcd'
is mapped to 1, and 'efgh' is mapped to 2.  To paste, just do Ctrl+V (at this point in our example, 'efgh' would be
pasted).

Right clicking on the notify icon in the system tray will bring up the options to Exit or Save.  Save will create
an xml file in the same directory as the exe that stores the data currently mapped to your keys.

By double clicking on the notify icon, the GUI portion of the app will open.  Here, you'll be able to see all
of the strings that have been mapped to your number keys.  You can switch between them here with the radio buttons;
this will have the same effect as doing a Ctrl+<number>.  Finally, there is a lock button near each field.  By clicking
on this button, a number will be "locked" or "unlocked."  If a number is locked, the text in it will not be overwritten,
either by editing in the GUI or by copying.  Use this option if there is a string you use regularly that you want to make
sure doesn't get lost.

Other than that, I hope you find this app useful.  

FULL DISCLOSURE: I don't normally work in .NET, so please don't kill me if there are any really terrible coding
practices in there :)