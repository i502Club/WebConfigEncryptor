# WebConfigEncryptor

WebConfigEncryptor is a DNN module that enables a host user to encrypt/decrypt the 
web.config data base connectionStrings section in single server environments. It 
uses the Windows Data Protection API (DPAPI) to encrypt and decrypt data. [Read more...](https://docs.microsoft.com/en-ca/previous-versions/aspnet/hh8x3tas(v=vs.100) 

Typically you will start with a config connectionStrings section that looks similar to this:
```
<connectionStrings>
    <add name="SiteSqlServer" connectionString="Data Source=XXXXXX;Initial Catalog=XXXXX;User ID=XXXXX;Password=XXXXXX"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
```

Your encrypted config will look simlilar to below:
```
 <connectionStrings configProtectionProvider="DataProtectionConfigurationProvider">
    <EncryptedData>
      <CipherData>
        <CipherValue>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</CipherValue>
      </CipherData>
    </EncryptedData>
  </connectionStrings>
```


## Getting Started
This DNN MVC module will enable any DNN 9.4+ site to encrypt their web config data base connection string. 
It's most useful for shared hosting environments where you may not have full access to the server. With 
WebConfigEncryptor you can still encrypt your db connection strings provided you have host access 
to your DNN installation. It can be also be used for dedicated servers, virtual machines or app 
services to simplify the task.


### About the Install
The module is a standard module installation.  There is no sql provider with this installation but 
it's best to back up your db and file sytem before installation. You should also backup 
your web.config before you're initial encrypt/decrypt attempts as a precaution and to verify that 
the module is performing as expected.

After installation place the module onto any page that is restricted to host users.  ***Only host users*** can encrypt or 
decrypt the web config connection string.  The module will log each encrypt & decrypt action to the DNN log.


### Installing
1. Backup your db and file system.  Install into DNN as a normal module.  The installation process will ensure that your 
DNN installation is using at least v9.4.0.  

2. Navigate to an existing admin page in your site or create a new page that is restricted 
to admins.  (ie. https://{mysite.com}/admin)

3. Place WebConfigEncryptor module on to the page.

4. There is just one button and clicking it will toggle the encryption/decryption depending 
upon the current state of your config.

---

*Congratulations*! The WebConfigEncryptor should now be ready and you can encrypt and decrypt 
your DNN db connection string on any server.

---

### Screenshot

![WebConfigEncryptor screenshot](assets/images/screenshot.png)

---

### Development
 1. Backup your db and file system.  Install the module into your development enviroment.
 2. Clone the repo to your /DesktopModules/MVC/ directory.
 3. Navigate to directory and open the .sln with Visual Studio.
 4. You should be able to compile and attach the debugger at this point.


## History
This module was used in house before it's public release at v1.0.0.  DI support was 
added to the module which requires an installation be Dnn version 9.4 or greater. 

## Authors
[![i502 Club](assets/images/icon_extension.png)](https://www.i502.club) [i502 Club](https://www.i502.club)

## License
This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details

## Acknowledgments
* All the contributors to [DNN](https://github.com/dnnsoftware/Dnn.Platform) 

## Contribute
You can create an issue or submit a pull request to help make the module better.