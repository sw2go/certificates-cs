##using certificates in c# application encrypt and decrypt with asymetric key-pair
tools: Powershell to create the certificate, C# to use the certificate 

To create a selfsigned certificates use powershell, see samples in "create-certificates.ps"

Create a *.pfx file:

1. Open Powershell console
2. Open "create-certificates.ps" and set variables $subject, $notafter and $filepath as you like 
3. Copy/Paste the text to the powershell console and execute it
4. Enter a password to protect the private-Key
5. Copy the thumbprint for later use

To install the certificate to the "local machine store" open command console as admin and type "certlm"
To install the certificate to the "current user store"  open "certmgr"
1. Select "Personal" folder
2. Right click "All Tasks" and "Import..."
3. Verify the correctness of the Store Location then click "Next"
4. Browse for the *.pfx or *.cer you just created before with powershell.
5. Enter password 
6. You may unselect all checkboxes, especially: DO NOT MAKE THE KEY EXPORTABLE then Click "Next"
7. Copy the thumbprint for use in C#

To use the certificate in C# use the thumbprint and code sample in Program.cs
To make the c# code work ensure that you provide the correct Location "CurrentUser" or "LocalMachine" 
and that the certificates reside in "My" i.e. "Personal" Folder of the location.

With a *.cer you can only "encrypt" and "validate a signature"  ... requires a public key only
With a *.pfx you can also "decrypt" and "create a signature"    ... requires also a private key




