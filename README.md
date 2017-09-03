# Arthware.X937Parser

At the office we had a requirement to parse check returns in an X9.37 file.
There is a commercial library/SDK for reading and writing X9.37 files.
I recommended against using the library for our requirement:
* For the purpose of interogating the X9.37 file for returns and returning an array of the discovered returns, we required only a very small subset of the libraries capabilities.  
* The API is "high friction" for implementing our simple requirement.  No pit of success here.
* The API is "file based", so it is necessary to copy the file to the file system in order to read the contents.
* The library costs money
* The library is not ".NET Core ready", has a bizzare dependency on WinForms
* The library deployment to production requires installing a license manager and manually entering licensing keys/codes.
* Deployment automation is ..er.. challenging
* For giggles I built a demonstration proof of concept project in a few hours that has all the functionality we needed, suitable for .NET Core, and "stream based" instead of "file based"

Regardless, the management decision was to use the library.  I won't expand on the reasoning there.

Since I built the demonstration project on my own time, I decided to open source it, in case anyone else has a similar requirement.

If this proves to be interesting then we can expand the functionality to parse other information out of X9.37 files, or whatever seems to be in demand.  If this proves to be crickets, then there won't be any expansion of functionality.

