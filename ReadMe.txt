Author of this project: 
Richard North
A0313108
19/12/2022

The Three Amigos Corp Web Application and Service
Welcome to our web services and web application project! 
This application is designed to provide users with a simple to navigate and intuitive interface for interacting with our Events Web. 
We also aim to provide a simple to use web service for booking venues and our dedicated catering company.
This project consists of two RESTful web APIs and a web application that consumes both web services to provide 
greater functionality to the user when creating food booking and reserving venues for the booked event.



Table of Contents:
	Design Purpose
	Installation
	Usage
	Development Issues

Design Purpose:
-- add in the purpose of this design --

Installation:
To install and run this project, you will need to have the following software installed on your computer:

.NET Framework
sqlite
Visual Studio (optional, but recommended for development)
Once you have these dependencies installed, follow these steps to set up the project:
Open the solution file (ThAmCo.sln) in Visual Studio.
Restore the NuGet packages by right-clicking on the solution and selecting "Restore NuGet Packages."
Build the Required databases by going to "Tools" > "NuGet Package Manager" > "Package Manager Console"
Open the Package Manager Console select "ThAmCo.Catering"
Run the following command in the terminal "Update-Database"
In the Package Manager Console select "ThAmCo.Venues"
Run the following command in the terminal "Update-Database"
In the Package Manager Console select "ThAmCo.Events"
Run the following command in the terminal "Update-Database"
Ensure that all projects are set to launch by right clicking "Solution 'ThAmCo'" > "properties" > "Multiple startup projects" and selecting all to start
Build the solution by going to "Build" > "Build Solution."
Run the application by going to "Debug" > "Start Without Debugging."

Usage:
To use the application, simply follow the prompts on the user interface.
Within the web application the user has the following abilities:

	Events:
	Create a new event.
	edit an events name and duration.
	View all upcoming events.
	Apply a soft delete to an event which will also soft delete all guest bookings and staffing for that event.
	recieve warning when an event does not have the required staffing level 
	recieve warning when there is no first aid qualified members of staff assigned to an event

	Guest:
	Create a new guest
	edit a guests details
	view all guests
	Delete a guest and all of their details
	anonymise a guest by completely removing all of their details and replacing their name with a "#"

	GuestBookings:
	Through the event page the user will be able to view all guest bookings asociated with that event
	add guests to an event
	delete a guests booking from the event 
	track a guests attendence for an event 
	update the guests attendence status

	Staff:
	Create a new staff member 
	edit a staff members details
	view all staff members currently employed by the Three amigos corp
	view an individual staff members details

	Staffing:
	assign a member of staff to an event 
	view all staff assigned to a particular event

Further specific details on how this functionality is achieved can be found be referring to method documentation and code comments throughout this project

Development Issues:
Here i will outline some of the development issues that were faced while designing this solution:
	Ajax:
	During the course of the development I had decided to employ the use of Ajax for a host of smaller tasks which included -
	calling the venues API to return the list of event types that are offered, calling the venues availability controller API to return a list
	of available venues that are free in the designated time frame e.g venues for a conference between the dates 12/12/2022 to 1/1/2023
	and finally to return all of the menus that the catering API has within their database to allow the user to easily create a new food booking 
	for the event. The introduction of this feature however did pose a few issues and ultimately took a couple of days to successfully resolve Some of the bugs 
    that this had generated. The bugs I encountered had appeared by a simple missing bracket. The bugs looked some what trivial but in hindsight were difficult to resolve due to
	the debugger provided within visual studio not providing code hints and error lists for javascript. This error was located wihin the loadAvailableVenues function
	which is located in the CreateEvent.js file. Also whilst developing this function I kept having an issue where the call to the get availability would not work. 
	This turned out to be an issue with the CORS not being enabled within the ThAmCo.Venues programe file which did not have this feature enabled. 
	This was discovered with the help of lecturers. This problem could have been solved by implementing a call for this data to have been made befor the view is generated 
	and this data being passed to the view on the controller method being activated but I had personally choosen to use Ajax for the oppourtunity to trail a technology 
	I had not yet been exposed to.

	JQuery:
	I had initially decided I would like to add a feature where when adding guests, the guests would appear in two seperate tables, one containing guests not currently assosiated 
	with the event and the other a table containing guests that are assigned to the event. I made it so the user could click on a button assigned to each row within the table 
	which would remove the guest from the not assigned table and place them in the assigned guest tables. When this was achieved I wanted to apply this in a similar way to
	the staffing for an event which would better diplay staff details than the current version. I had created a jquery to dynamically create these which worked well however, 
	I was unable to find a way in which I could access the individual guests, track which buttons had been clicked and also update the correcsponding guests associated within 
	that event. 