# Use case stories

## File Hosting like dropbox app

`Accounts control`
- New users will be able to register and login using their `email address` and `password`
- Users with accounts can log in using their `email address` and `password`

`Core functionality`
- create _new folder_
- rename or keep the _new folder_ name
- open computer harddrive folders
- upload file(s)
- show file name(s) as a list
- delete functionality on `files` and `folders`
- share files with other users


## Domain/Entity
1. File - Object to be stored/uploaded
2. Folder - Space used as a directory to store the files
3. User - Owner of the account used to create the `folder` and store the `files`


## SIMPLE UML DIAGRAM

The suggested diagram below shows a basic classes and relationships for a file hosting tool like dropboxlike. The `file` class represents a file, with properties such as _name_, _size_, _created date_ and _modified date_. The `user` class represents a user, with properties such as _name_, _email_ and _password_. The `FileHostingService` class in this case represent for example, AWS S3 which stores file and provides methods for _uploading_, _downloading_ and _deleting_ files. The `Client` class represent the frontend that connects the hosting file and uses it to _upload_, _download_ and _delete_ files.

##### TODO : Add support for sharing files, permission etc

[File]
- name: String
- size: Int
- createdAt: Date
- modifiedAt: Date

[User]
- name: String
- email: String
- password: String

[FileHostingService]
- users: List<User>
- files: List<File>
- uploadFile(file: File): void
- downloadFile(file: File): void
- deleteFile(file: File): void

[Client]
- connectToFileHostingService(service: FileHostingService): void
- uploadFile(file: File): void
- downloadFile(file: File): void
- deleteFile(file: File): void


Check out [UML Drawing](https://lucid.app/lucidchart/49967d8f-869a-4757-bb3f-8ada14ea7cde/edit?invitationId=inv_c7ee6e19-70c1-4fc1-9302-c859686f2972)