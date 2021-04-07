# WebInterface

The Setup:

1. Create data/Web path. this will contain (htmls,css,and any other website related files)

2. (Optional) Add .fileset files to restrict access to certain files
These Files Contain Json similar To the example:

{
  "fileAccessess": [
    {
      "fileNames": [
        "example.html"
      ],
      "accessType": "example"
    }
  ]
}


The Interfaces Are Optional but the example shows what you can do with them

Every Interfaces Takes There Parameters

Port: sets to which port this interfaces will listen (set to 0 for all)

Path: sets at what path the interface will react to ( if * is used it will trigger for any file located in folder path)

Method: sets to what method the interface will react to ( leave blank for any)

