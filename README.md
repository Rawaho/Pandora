## Pandora
### Information
Pandora is a server emulator for the CCG Faeria that supports patch 1.0.6313.26946  

### Getting Started
*It's recommended that you perform these steps on separate Faeria installation as they will prevent you from connecting to the offical server.*  
* Before you can build the solution you'll need to copy the Faeria C# assembly from `...\Faeria\Faeria_Data\Managed\Assembly-CSharp.dll` to the repository directory `...\Source\Dependencies\Assembly-CSharp.dll`.  
* Once the solution is built, to patch the client run `PandoraPatcher.exe` found in the build output folder `Source\bin` with the Faeria install directory as the parameter `PandoraPatcher "C:\Program Files (x86)\Steam\steamapps\common\Faeria"`.  
* With the client patched you can create a new MySQL database and apply the file `...\Database\Base\WorldDatabase.sql`, you'll also need to apply any additional files found in `...\Database\Updates`.  
* Remove the .example extension from `Config.json.example` in the build output folder and set the relavent MySQL database information in the config.  
* Starting `Pandora.exe` should now successfully start the server, you can either launch the client through Steam or start manually from  `Faeria.exe`.  

### Requirements
 * [Visual Studio 2015-2017 (C# 6)](https://www.visualstudio.com/)
 * [MySQL Community Server](https://dev.mysql.com/downloads/mysql/) (or equivalent database)
