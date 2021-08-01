AsaanApply - Web Development Project

Tools: ASP.NET, MVC

The app has a homepage where information of the university is available and different tabs which user can click to perform different actions.
The user can sign sign up for registration.
After signing up, they are required to log in with their username and password.
All the fields have relevant javascript validations attached to them so the user cannot enter invalid data in the fields.

After signing in, they can see a student dashboard infront of them which contains their information and current application status. The user can click on Profile and change their basic info as well.
They can then navigate to the tab named application form which contains multiple sections. The students are required to fill in all the sections to be able to submit their admission application.

After the application is submitted, the students can navigate to the status tab in the left pane to see the status of their application. For testing purposes we generated a challan which the user can download to pay for their test fee.

Apart from this, there is another interface for admins. This one can be accessed only by admins.
The admins can set boundaries and limitations to the account options for students and can also add information regarding the university on the website.

One catch is that if you run the application on a new system, you would need to run the admin interface first and set the values for dropdown options in the Student accounts.
Only after this students will be able to access their interfaces. This is done to keep generality as in future we could have to design a similar application for a different university so we don't have to change the code evreytime and are able to change information from the admin interface.

To run this application, you just need a stable version of visual studio, perferably 2015 or above and a browser e.g. Google Chrome. Just open the sln file in android studio and click run or press Ctrl + F5.

