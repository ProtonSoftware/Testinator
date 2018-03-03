<?php
	
	// Get current version from Testinator App
	$currentVersion = $_POST['version'];
	
	// Get application type as well
	$applicationType = $_POST['type'];
	
	// Get newest version from file
	$filename = "version_" . $applicationType . ".txt";
	$fileContent = file_get_contents($filename);

	// Check if an update is necessary
	$isUpdateNecessary;
	$versionlength = strlen($fileContent);
	if ($fileContent[$versionlength - 1] == '!')
		$isUpdateNecessary = true;
	
	// Get the newest version based on that
	$newerVersion = ($isUpdateNecessary == true) ? rtrim($fileContent,"!") : $fileContent;

	// Compare both versions
	$doUpdate = version_compare($currentVersion, $newerVersion, '<');

	// Return the result as file to simplify C# app reading
	$result;
	if ($doUpdate)
	{
		echo 'New update';
		echo $isUpdateNecessary ? ' IMP' : '';
	}
	else
		echo 'No update';

?>