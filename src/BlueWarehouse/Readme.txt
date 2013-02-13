:: Solution

	> BlueWarehouse
	  This is the core project, i.e. the UI.

	> BlueWarehouse.Model
	  Domain model project. Contains model classes and structs, i.e. empty data containers.
	  No logic.

	> BlueWarehouse.Service
	  Business logic.

:: Debugging

	The natural way to debug BW is to point it toward Bamboo and actually have it handle a package.
	To do so, you will need to enter a proper set of parameters/flags in the "BlueWarehouse" project
	properties.

	1.	Right click the "BlueWarehouse" project, select "Properties". Then, select the "Debug" tab.
	2.	In the "Command line arguments" box, enter your parameters. It could look something like
		the following.
		--job=JOB1 --project=TV4 --plan=L4STAGE --artifact=l43-latest-(full) --target="C:\temp\bla"

	Now, each time you debug or start the solution, BW will run with these parameters set!