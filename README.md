Below is the instruction for the offline coding try-out.
Kindly advise the candidate to push it on GitHub and share the link once done.
Preferably, to be submitted within 3 days.

Long-running job

• Bare minimum requirements:
o Provide us with your example of the simple SPA with .Net 6 on the backend,
and Angular/ReactJS on the front-end. On the front-end side user should be
able to enter the text into the text field, press the "Convert" button, and get
this text encoded into the base64 format.
Encoding should be performed on the backend side. Encoded string should
be returned to the client one character at a time, one by one, and for each
returned character there should be random pause on the server 1-5 seconds.
All received characters should form a string in a UI textbox, hence it will be
updated in real-time by adding new incoming characters. User cannot start
another encoding process while the current one is in progress, but user can
press the "cancel" button and thus cancel the currently running process.

• Bonus (recommended) requirements:
o Web page should look neat; use Bootstrap or its derivatives
o Use default IoC, package managers and other tools to build & run the app
o Use the latest released .Net & C# with all possible new features they provide
o Server-side app should be hosted in Linux Docker container
▪ Host API & UI backend in different containers
▪ Support basic authentication using nginx in another container
o Business logic should be implemented as a service, with unit tests for each
respectively
o No compiled/build stuff should be provided, only source code is required

Example of the app functioning:
• input = "Hello, World!". Generated base64="SGVsbG8sIFdvcmxkIQ=="
o What web client receives from the server:
▪ Random pause... "S"
▪ Random pause... "G"
▪ Random pause... "V"
▪ Random pause... "s"
▪ Etc.
o What does user see in the result text field on web UI:
▪ Random pause... "S"
▪ Random pause... "SG"
▪ Random pause... "SGV"
▪ Random pause... "SGVs"
▪ Etc.