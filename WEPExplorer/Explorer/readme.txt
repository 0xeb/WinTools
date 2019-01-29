===================
Setup
===================
Compile the "cli" project and copy the produced "cli.exe" tool to the "Explorer\Tools" folder.
The external utility "cli.exe" will be used by the Explorer GUI.


===================
GUI Usage
===================

1. Run the Windows Events Providers Explorer from a directory with write-permissions.
2. Double click (or press ENTER) on a provider name to list its metadata
3. Play with the providers filters and then press "Apply" to update the metadata listing
4. Press Ctrl-I in the metadata listing to view the metadata template XML information

===================
Troubleshooting
===================


Deleting the cache
--------------------
The Explorer utility creates a folder called "Providers". It contains the metadata XML cache files for all the providers (All.xml) and for individual providers.
You may want to delete this folder to clear the cache and start over