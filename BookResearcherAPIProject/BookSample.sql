
--Authors
INSERT INTO Authors (AuthorID, Name) VALUES (1, 'Goswami, Jaideva');
INSERT INTO Authors (AuthorID, Name) VALUES (2, 'Foreman, John');
INSERT INTO Authors (AuthorID, Name) VALUES (3, 'Hawking, Stephen');
INSERT INTO Authors (AuthorID, Name) VALUES (4, 'Dubner, Stephen');
INSERT INTO Authors (AuthorID, Name) VALUES (5, 'Said, Edward');
INSERT INTO Authors (AuthorID, Name) VALUES (6, 'Vapnik, Vladimir');
INSERT INTO Authors (AuthorID, Name) VALUES (7, 'Menon, V P');
INSERT INTO Authors (AuthorID, Name) VALUES (8, 'Shih, Frank');
INSERT INTO Authors (AuthorID, Name) VALUES (9, 'Konnikova, Maria');
INSERT INTO Authors (AuthorID, Name) VALUES (10, 'Sebastian Gutierrez');

--Books 
INSERT INTO Books (BookID, Title, ISBN) VALUES (1, 'Fundamentals of Wavelets', '0195153448');
INSERT INTO Books (BookID, Title, ISBN) VALUES (2, 'Data Smart', '0002005018');
INSERT INTO Books (BookID, Title, ISBN) VALUES (3, 'God Created the Integers', '0060973129');
INSERT INTO Books (BookID, Title, ISBN) VALUES (4, 'Superfreakonomics', '0374157065');
INSERT INTO Books (BookID, Title, ISBN) VALUES (5, 'Orientalism', '0393045218');
INSERT INTO Books (BookID, Title, ISBN) VALUES (6, 'Nature of Statistical Learning Theory', '0399135782');
INSERT INTO Books (BookID, Title, ISBN) VALUES (7, 'Integration of the Indian States', '0425176428');
INSERT INTO Books (BookID, Title, ISBN) VALUES (8, 'Image Processing & Mathematical Morphology', '0671870432');
INSERT INTO Books (BookID, Title, ISBN) VALUES (9, 'How to Think Like Sherlock Holmes', '0679425608');
INSERT INTO Books (BookID, Title, ISBN) VALUES (10,'Data Scientists at Work', '074322678X');

--LibraryBranches
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (1, 'Agincourt Library', '155 Bonis Ave', 'Toronto', '416-396-8943');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (2, 'Albion Library', '1515 Albion Road', 'Toronto', '416-394-5170');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (3, 'Bendale Library', '1515 Danforth Road', 'Toronto', '123-456-7892');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (4, 'Library', '101 Eastside St', 'Toronto', '123-456-7893');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (5, 'Library', '202 Westside St', 'Toronto', '123-456-7894');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (6, 'Library', '303 Northside St', 'Toronto', '123-456-7895');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (7, 'Library', '404 Southside St', 'Toronto', '123-456-7896');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (8, 'Library', '505 Lakeside St', 'Toronto', '123-456-7897');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (9, 'Library', '606 Hilltop St', 'Toronto', '123-456-7898');
INSERT INTO LibraryBranches (LibraryID, Name, Address, City, ContactNumber) VALUES (10, 'Library', '707 Valley St', 'Toronto', '123-456-7899');

--BookAuthors
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (1, 2);  -- 1984 by George Orwell
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (2, 3);  -- The Great Gatsby by F. Scott Fitzgerald
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (3, 4);  -- The Hobbit by J.R.R. Tolkien
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (4, 5);  -- To Kill a Mockingbird by Harper Lee
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (5, 6);  -- Pride and Prejudice by Jane Austen
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (6, 7);  -- Oliver Twist by Charles Dickens
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (7, 8);  -- War and Peace by Leo Tolstoy
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (8, 9);  -- The Old Man and the Sea by Ernest Hemingway
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (9, 10); -- The Adventures of Tom Sawyer by Mark Twain
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (10, 1); -- Harry Potter and the Sorcerer’s Stone by J.K. Rowling

--BookAvailability
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (1, 1, 5);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (2, 2, 3);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (3, 3, 4);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (4, 4, 2);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (5, 5, 6);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (6, 6, 1);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (7, 7, 3);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (8, 8, 2);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (9, 9, 4);
INSERT INTO BookAvailability (BookID, LibraryID, Quantity) VALUES (10, 10, 5);
