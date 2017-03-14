# MyTAsks
Simple Ticket web app.

This is a beta version not finished yet. 

# Requirements (for developers)

Install Mysql,IIS,VS2015

# Run it from VS 2015

Before run the application from VS, you need to restore nuget packages doing right click in the solution and choose :"Restore Nuget packages"

# Creating the Database (Web.config)

You need to open the web.config file and use your credentials in the section "ConnectionStrings". 
RootConnection will contain a user with rigths to create databases in mysql.
DefaultConnection will be used to connect to the mysql database used by MyTasks.

# Run it in the browser

After have the correct connectionstring you can run from VS and go to the url: - localhost:58070/Install/FirstInstall (and press in Create and Install database).


# Demo Url

http://mytasks.biglamp.cl.
You can use rolandomartinezg@mytasks.cl/ password to log in. From there you can see some demo data and demo users.






