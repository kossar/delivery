# delivery

This project I had to make in Taltech, main subject was Distributed systems, also We shared same project with ASP.NET subject. 
And since I took also JS subject, then most of JS homeworks are using this project API controllers.

# Distributed systems homeworks


# HomeWork 1

All course projects (there will be several) in single git repo.
https://gitlab.cs.ttu.ee  
Git repo has to be named: **icd0009-2020s**  
Every project in its own directory.  
Do not commit binaries and platform specific metafiles to git. Use correct .gitignore file.  
Combine these as a starter:  
https://raw.githubusercontent.com/github/gitignore/master/VisualStudio.gitignore  
https://raw.githubusercontent.com/JetBrains/resharper-rider-samples/master/.gitignore

## Leg 1

### Project proposal

DeadLine **25.02.2021 23:59:59**  

Expected project size:  
Minimal 10 entities, not counting many:many in-between tables, logging, translations and identity tables (user, role, user_in_role, etc).  

### Code

Domain models + MVC WEB CRUD controllers.  
User, Role and UserRole are provided by framework.  So user creation, login, etc - we use framework provided functionality. Base class for users is IdentityUser - you just inherit from that  (or define your own initially).  

### Documentation

Language: English (preffered) or Estonian.  

Full ERD Schema (QSee, UML, ....) - including attributes (use vertabelo, qsee, lucidchart, etc).  
Project description - what, why, what is your motivation, how will world be better, etc.  Basically final thesis intro.  
Actual content - minimum one A4.  
All visual documents as source and pdf-s in git.  
Pdf also uploaded to course moodle (https://moodle.taltech.ee/mod/assign/view.php?id=358995).  


Add main client flow proposed screens (use some prototyping tool - figma, adobe xd, sketch, axure, invision, etc. - digitized pencil and paper is also ok).  
For example wolt.ee - Home screen, restoran discovery, order proccess, payment, delivery.  

If you dont have your own project idea, inmplement/plan this:  
Restaurant food ordering system (like wolt/bolt), non-profit. Restaurants can set up their own data and clients can order and do their own pick-up.


Project description - using of school written policy is mandatory.  
https://www.ttu.ee/public/i/infotehnoloogia-teaduskond/Tudengile/Vormid/ITT_loputoode_juhend_EST.pdf  
https://www.ttu.ee/public/i/infotehnoloogia-teaduskond/Tudengile/Vormid/FIT_Author_Guidelines_ENG.pdf  

Andres Käver ramblings about final thesis - for some inspiration (in estonian):  
https://enos.itcollege.ee/~akaver/FinalThesis%20guide/

English by google translate:
https://translate.google.com/translate?hl=en&sl=et&tl=en&u=https%3A%2F%2Fenos.itcollege.ee%2F~akaver%2FFinalThesis%2520guide%2F

Your project document's need to stay in sync as your project evolves.  
When you need to switch a project topic (which is totally ok) - project documentation needs to be rewritten.  

# HomeWork 2

## Leg 1

### Repositories and generics

DeadLine **11.03.2021 23:59:59**  

Implement Repositories (using generics and interfaces for all your entities).


### Code

Upgrade all your controllers to use repositories.




## Leg 2

### Unit of Work

DeadLine **18.03.2021 23:59:59** 

### Code

Upgrade all your controllers to use Unit of Work. Do not reference repositories directly (only via UOW).  
Replace AppDbContext with UOW in all the controllers. Register and inject UOW via DI. 


# HomeWork 3

## Identity

### Leg 1

DeadLine **25.03.2021 23:59:59**  

Implement custom identity in your code. Extend IdentityUser and IdentityRole as needed. Update your documentation and ERD schemas accordingly. Update identity UI as needed so user creation and profile changes work correctly.
Fix all the nullable-ref problems in identity code!

Implement user and role management. 
Only users in Admin role should be able to see and access the UI for it. They can block/unblock users and assign/remove them from roles. And manage roles (crud).
Use usermanager/rolemanager for the operations.

Use this in ConfigureServices. Provides correct DI for RoleManager<AppRole>

~~~csharp
    services
        .AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddDefaultUI()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
~~~


# HomeWork 4

## Identity and Uow in rest api controllers

### Leg 1

DeadLine **01.04.2021 23:59:59**  


Implement JWT based identity system in your API controllers.
Create controller for register and login functionality. Use DTO-s.


# HomeWork 5

## BLL and Mappers

### Leg 1

DeadLine **08.04.2021 23:59:59**  


Implement BLL and mappers in all levels. Including mappers for Public DTOs out from rest controllers.


# HomeWork 6

## API versioning and documentation

### Leg 1

DeadLine **21.04.2021 23:59:59**  


Implement XML comments in your Web project (all the Api controllers/methods have to be commented).  
Implement Swagger/OpenApi attributes in all your REST controllers.  

For example:

~~~csharp
// GET: api/Simples
/// <summary>
/// Get all the simples
/// </summary>
/// <returns>List of Simples</returns>
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(typeof(IEnumerable<Simple>), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public async Task<ActionResult<IEnumerable<Simple>>> GetSimples()...
~~~        

Compilation of your solution must show 0 (zero) warnings.  
Implement Api versioning and Swagger documentation and UI.  

# HomeWork 7

## i18n support

### Leg 1

DeadLine **28.04.2021 23:59:59**  

Implement RequestLocalizationOptions and set up default culture.

You do not need to translate anything in the UI.  
NB! This task is bigger in asp.net course (translations.)


# HomeWork 8

## Docker support and hosting

### Leg 1

DeadLine **05.05.2021 23:59:59**  

Dockerize your solution, publish the image and host it from some cloud service.  
Azure, AWS, etc. Try to use your student status (free credit).  
Set up webhook in your Docker Hub to automatically publish the image into hosting in case of image update.  
Include the url where your app is accessible in your git repo README.md


# HomeWork 9

## Testing - unit and integration tests

### Leg 1

DeadLine **Project Defence**  

Unit Tests  
Cover (100% code coverage) one (the main one!) custom service and base service with unit tests.


Integration tests  
Cover main happy flow through your web app with integration test.


# ASP.NET Subject specific homeworks for same project

# HomeWork 1, 2, 3

# ONLY IF YOU HAVE DECLARED ASP.NET ALONE (without Distributed Apps cource)
If you take both courses and share the project - use Distributed repo


DeadLine **25.03.2021 23:59:59**  

Implement all Distributed Apps Development homeworks HW1, HW2 and HW3 (most likely you share the projects).

# HomeWork 3

## Identity

### Leg 1 (in addition to Distributed spec)

DeadLine **25.03.2021 23:59:59**  

Implement user/role management in seprate area.
Allow only users in specific role ("admin") to access and see user management (including the dropdown menu in layout).  
Implement data seeding for default admin user (user info must be specified in appsettings.json).


# HomWork 4 - distributed specific

# HomeWork 5 - identical to distributed course

# HomeWork 6 - identical to distributed course

# HomeWork 7

## i18n support

### Leg 1

DeadLine **28.04.2021 23:59:59**  

Implement RequestLocalizationOptions and set up default culture.

Translate everything! You need to support at least two languages - English and one another (pick whatever you like). All the error messages, all the UI - including the identity. EVERYTHING!.


### Leg 2

DeadLine **05.05.2021 23:59:59**  

Translate your database content (domain entities)!  
Support unlimited amount of languages.  


### Leg 3

DeadLine **Project Defence**  

Implement full clientside js build mechanism.  
Replace date, date-times, times with js UI.  
Replace jquery validation with Globalize based validation.  
If you dont have any date-time-datetime fields - just do simple demo with them somewhere (most likely you have some somewhere and admin can modify these).



