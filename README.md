# Arthware.X937Parser

At the office we had a requirement for a microservice to parse check returns from an X9.37 file and emit metadata about each return.
There is a commercial library/SDK for reading and writing X9.37 files.
I recommended against using the library for our requirement:
* For the purpose of interogating the X9.37 file for returns and returning an array of the discovered returns, we required only a very small subset of the libraries capabilities.  
* The API is "high friction" for implementing our simple requirement.  No pit of success here.
* The API is "file based", so it is necessary to copy the file to the server file system in order to read the contents.
* The library costs money
* The library is not ".NET Core ready", has a bizzare dependency on WinForms
* The library deployment to production requires installing a license manager Windows application and manually entery of licensing keys/codes.
* Deployment automation is ..er.. challenging
* For giggles, in my personal time, I built a demonstration proof of concept project in a few hours that has all the functionality we needed, suitable for .NET Core, and "stream based" instead of "file based"

Regardless, the management decision was to use the library.  I won't expand on the reasoning there.

Since I built the demonstration project on my own time, I decided to open source it, in case anyone else has a similar requirement.

If this project proves to be interesting in the wild then we can expand the functionality to parse other information out of X9.37 files, or whatever seems to be in demand.  If this project proves to be crickets, then there won't be any expansion of functionality.

MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.


