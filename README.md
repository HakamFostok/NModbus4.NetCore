## What is NModbus4.NetCore?

Very simply, it is [**NModbus4**](https://github.com/NModbus4/NModbus4) build for .NET (**NOT .NET Framework**)<br/>

## OK, So what is NModbus4 in the first place?
Here is a snippet from NModbus4 documentation 
>NModbus is a C# implementation of the Modbus protocol. Provides connectivity to Modbus slave compatible devices and applications. Supports serial ASCII, serial RTU, TCP, and UDP protocols. NModbus4 it's a fork of NModbus(https://code.google.com/p/nmodbus). NModbus4 differs from the original NModbus in following:
>1. removed USB support(FtdAdapter.dll)
>2. removed log4net dependency
>3. removed Unme.Common.dll dependency
>4. assembly renamed to NModbus4.dll
>5. target framework changed to .NET 4

## Why NModbus4.NetCore?

Because 
1. The original library is not under active development, the last commit is dated 5 years ago <br/>
2. There is a need to use **NModbus4** with .NET projects, but **NModbus4** support only .NET Framework but the original library does not support .NET <br/>

## What is the difference between NModbus4 and NModbus4.NetCore?

**NModbus4.NetCore** is a copy of **NModbus4** with the following differences

1. As mention earlier, The main difference is the Target Framework.
2. **NModbus4.NetCore** supports the SerialPort out-of-the-box. <br/>
   **NModbus4** needs some configuration to support SerialPort that (by Defining the Compile-time constant).
3. Supporting .NET with this library does **NOT** mean expanding the **NModbus4** <br/>
   I mean **NModbus4.NetCore** will work **only** with .NET, <br/>
   **if you want to use NModbus4.NetCore with .NET Framework you are in the wrong place, go to the original library then**

## Nuget Package<br/>

https://www.nuget.org/packages/NModbus4.NetCore

    Install-Package NModbus4.NetCore

or

    dotnet add package NModbus4.NetCore

or

    <PackageReference Include="NModbus4.NetCore" Version="1.2.0" />


## Roadmap
The followings are the enhancement that I plan to add
1. Use the latest C# constructs
2. Add CI/CD by using Github action to automatically publish Nuget Package
3. Add Coverage badge and Nuget badge
4. Visual Studio now support the debugging of the Open source library<br/>
   I added this feature but I did not publish it to the Nuget, <br/>
   I will publish that ASAP


## Contributaions
1. PRs is more than Welcome.
2. I do not care about the style of commit message <br/>
   just describe what you did in a meaningful way, in the style you like.
3. You do not have to format the code.<br/>
   I am obsessive with formatting the code, and I will format the whole solution when merging any PR.
4. Do not worry if you code with other style, I will change the style to accomodate mine <br/>
   So you do not have to worry about changing your current style. 
4. If you can help with the CI/CD operations, it is more than welcome, I do not have that much experience with that.
5. Any PR which downgrade the TFM or changing the C# code to make it use older C# constructs **will be rejected**