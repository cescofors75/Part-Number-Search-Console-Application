Part Number Search Console Application
This repository contains a console application for searching and managing part numbers. Users can interactively search for part numbers, view related EANs, and check whether a part number is stocked.

Features
Interactive Interface: Users are prompted to enter a part number, and the application responds with relevant information.
List Stocked Part Numbers: By entering the command list, users can view a list of all part numbers currently stocked.
View EANs: By entering the command ean, users can view a list of EANs associated with part numbers.
Exit Command: Users can exit the application by entering the command exit.
Colored Output: The application uses different colors to distinguish between part numbers that are stocked and those that are new.
Installation & Setup
Clone the repository:

bash
Copy code
git clone [repository-link]
Ensure that you have the required dependencies, such as the necessary database context and models.

Compile and run the application.

Usage
Launch the compiled console application.
Follow the on-screen prompts to enter a part number or use one of the available commands.
View the results displayed on the console.
Dependencies
Entity Framework: For database interactions (if used).
Custom Models: The application relies on custom models for data representation.
Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

License
MIT

