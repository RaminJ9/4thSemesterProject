Guide to setting up a new component (Visual Studio. idk JetBrains).


## Create new Project
Right click 'Solution back' -> Add -> New Project
(I think 'Class Library' is a good template option)

## Add NuGet
Right click project (eg. 'Example' in this example) -> Manage NuGet packages
In the GUI, Click 'Browse'
Search for 'System.Composition'
Install it (Latest)
Accept all popups

## Add Common refference
Right click project (eg. 'Example' in this example)
Click Add -> Add project refference
In GUI, Add Common (not any others)

## Using Common interfaces
When making your machine class, add this on the line above your class
```C#
[Export(typeof(IMachineComponent))]
public class AGV
```

## Done
Start coding :)
