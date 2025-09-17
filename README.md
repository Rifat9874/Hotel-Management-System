# Hotel-Management-System
Case Study: Successful Implementation of Hotel Management System (HMS)
A mid-range hotel faced challenges in managing high booking volumes, ensuring room availability, and improving staff communication. To address these, the hotel implemented the Hotel Management System (HMS), which automated key processes like room bookings, payment processing, and room assignments, ensuring seamless operations and real-time synchronization across departments.
With the HMS, receptionists efficiently managed check-ins, check-outs, and room status updates in real time, reducing delays and preventing overbooking. The system’s dashboard displayed booking ID, guest details, and room assignments, streamlining operations. Receptionists could also cancel or modify bookings quickly, enhancing guest satisfaction.
For housekeeping staff, the system enabled room assignments, maintenance tracking, and task management. Staff could mark rooms as cleaned and report maintenance issues, triggering automatic alerts to administrators, who monitored progress and resolved issues faster.
Administrators gained better control with dashboards displaying critical stats like daily revenue, room occupancy, and bookings. They could approve leave requests, ensuring the hotel remained adequately staffed, and generate revenue reports for better pricing and strategy optimization. The system also provided secure payment options (Card, Bkash, Rocket, Nagad, Bank Transfer) and automated PDF receipt generation for both customers and the hotel’s record-keeping.
The HMS enhanced customer experience with a review system, allowing guests to rate their stays and book additional services like the pool, spa, and buffet. By digitizing manual processes, the HMS reduced errors, improved resource allocation, and boosted staff productivity.
Overall, the HMS transformed the hotel’s operations, improving operational accuracy, reducing manual tasks, and enhancing guest satisfaction, making it an ideal solution for hospitality businesses seeking efficiency and scalability.

                                      Features List  
                                      
1. User Authentication & Management
•	Registration: New users can sign up with name, email, phone, and password.
•	Login: Role-based login (Customer, Receptionist, Staff, Admin).
•	Password Recovery: Forgot password functionality via email and phone verification.
•	Profile Management: Users can view and edit their personal information.
2. Customer Features
•	Room Booking:
o	View available rooms with images, descriptions, and pricing.
o	Filter and sort rooms (alphabetically, price low-high, price high-low, Booked, Available ).
o	Select check-in/check-out dates.
o	Confirm bookings with payment.
•	Booking Management:
o	View booking history.
o	Cancel bookings.
•	Multiple Payment:
o	Card/Bkash/Rocket/Nagad/Bank Transfer
o	 Generate OTP
o	Secure payment processing.
o	Generate and download PDF receipts.
•	Write a Review 
o	Rating System
o	Comment section 
o	Review Submission
o	Booking Verification
•	Hotel Facilities :
o	Browse amenities like pool, buffet, and spa with brief descriptions.
o	Transparent per-person pricing shown.
o	Easy booking via “Book Now” for each facility.
o	
•	Edit Info & Unpaid Pay Bill 
o	Easily update your details
o	View and manage unpaid bills through a user-friendly online portal.
3. Receptionist Features
•	Booking Management:
o	View all active bookings.
o	Update booking status (Booked → Check-In → Check-Out).
o	Remove bookings if needed.
•	Leave Application:
o	Apply for leave with dates and reason.
o	View leave request history and status.
•	Dashboard View:
o	Booking ID
o	Name
o	Email
o	Room Number
o	Check-In and Check-Out dates
•	Sorting Options:
o	Default Sorting
o	Check-In
o	Paid
o	Pending
o	Booked
o	Check-Out
•	Profile Features:
      Profile Overview: Displays and allows editing of
o	Name
o	Email
o	Phone Number
       Tabs :
o	Salary Info: View salary details.
o	Additional Info: Displays extra relevant information.
o	Leave Info: Shows leave history, remaining leave, and statuses (Pending, Approved).
4. Staff Features
•	Staff Dashboard
o	Description: Displays the staff's tasks for the day, showing assigned rooms, status (cleaned/dirty), and assigned dates.
•	Room Assignments:
o	View assigned rooms and cleaning status.
o	Mark rooms as cleaned.
•	Maintenance Requests:
o	Report room maintenance issues.
•	My Tasks :
o	 A list of tasks assigned to the staff with options to mark tasks as completed. Includes an option to report room issues.
•	Staff Profile
o	Displays the staff's profile with personal information, job details, and the option to edit the info. Includes tabs for additional info, salary, and leave details.
•	Salary Info:
o	Shows the staff's salary details, including salary amount, allowances, deductions, bonuses, and net salary. Also provides the last salary date and payment method.
•	Leave Management:
o	A list of tasks assigned to the staff with options to mark tasks as completed. Includes an option to report room issues.
5. Admin Features
•	Dashboard Overview:
o	Key Statistics: Displays important data like Today's Check-Ins, Total Bookings,    Occupied Rooms, and Today’s Revenue.
o	Recent Check-ins & Check-outs: Shows the latest check-ins and check-outs with customer details, room numbers, and booking dates.
•	Leave Request Handling:
o	Approve or dismiss staff/receptionist leave requests.
•	Maintenance Management:
o	Room Details & Price: Displays the list of rooms with their types, prices, and the option to update them.
o	Maintenance Issues: Tracks issues in rooms, their status (pending, in progress, completed), and provides the ability to mark them as resolved.
•	Revenue Report:
o	Generates a revenue report showing daily ,monthly,yearly,bookings and revenue. Allows for the selection of a date range and report type.
•	Customer Reviews:
o	Customer Feedback: Displays reviews with ratings, comments, and the option to delete selected reviews.
•	Facilities Bookings:
o	Facility Reservation: Manages facility bookings (e.g., swimming pool, spa), showing the booking status (pending, confirmed, completed), guests count, and total price. Admin can update the status.
•	Booking Reports Enhancement:
o	Check-In & Check-Out Dates
o	Customer Details:
o	Room Details
o	Booking Amount
•	User Management:
o	(Add/remove users (staff, receptionists).
o	Update salary 
o	Add Bonus 
Technical Features
•	Database Integration:
o	SQL Server for storing user data, bookings, rooms, etc.
•	Dynamic UI Generation:
o	Rooms, bookings, and assignments displayed in dynamically generated panels.
•	PDF Receipt Generation:
o	iTextSharp for creating payment receipts.
•	Role-Based Access Control:
o	Different dashboards for customers, receptionists, staff, and admins.
•	Responsive Design:
o	Adjusts UI elements based on screen size.
Additional Features
•	Room Details:
o	View room images, amenities, and features.
•	Search Functionality:
o	Search rooms by type.
•	Date Validation:
o	Prevents booking conflicts by checking room availability.

                                      Schema Diagram
   <img width="1065" height="683" alt="image" src="https://github.com/user-attachments/assets/c60f79b3-ba6a-47de-842d-eb5472a1e1b3" />

                                       ER  DIAGRAM
<img width="1079" height="1230" alt="image" src="https://github.com/user-attachments/assets/0bf1412e-12a4-4766-bdee-a5a30fe817b1" />


 
 
                                  System Functionalities with SQL Queries
ADMIN DASHBOARD:

   1.LOAD ROOMS
SELECT RA.ROOM_ID, SA.NAME
FROM BOOKINGS_TABLE RA
JOIN USER_TABLE SA ON RA.EMAIL = SA.EMAIL
WHERE RA.BOOKING_STATUS = 'CHECK-IN'
   2.       LOAD BOOKING HISTORY 
SELECT RA.ROOM_ID, RA.BOOKING_DATE, SA.NAME
FROM BOOKINGS_TABLE RA
JOIN USER_TABLE SA ON RA.EMAIL = SA.EMAIL

   3.LOAD DASHBOARD COUNTS:
            -- Total Bookings
SELECT COUNT(*) FROM BOOKINGS_TABLE
-- Occupied Rooms
SELECT COUNT(*) FROM BOOKINGS_TABLE WHERE BOOKING_STATUS = 'CHECK-IN'
-- Today's Check-Ins
SELECT COUNT(*) FROM BOOKINGS_TABLE WHERE CAST(CHECK_IN AS DATE) = CAST(GETDATE() AS DATE)
-- Today's Revenue
SELECT ISNULL(SUM(DATEDIFF(DAY, B.CHECK_IN, B.CHECK_OUT) * R.PPN), 0) 
FROM BOOKINGS_TABLE B
JOIN ROOM_TABLE R ON B.ROOM_ID = R.ROOM_ID
WHERE CAST(B.BOOKING_DATE AS DATE) = CAST(GETDATE() AS DATE)
    4.LOAD REVIEWS 
SELECT
R.REVIEW_ID,
U.NAME AS CustomerName,
B.ROOM_ID,
R.RATING,
R.COMMENTS,
R.REVIEW_DATE,
B.BOOKING_ID,
U.EMAIL
FROM REVIEWS_TABLE R
JOIN USER_TABLE U ON R.EMAIL = U.EMAIL
JOIN BOOKINGS_TABLE B ON R.BOOKING_ID = B.BOOKING_ID
ORDER BY R.REVIEW_DATE DESC
    5.LOAD FACILITY BOOKINGS
             SELECT 
    FB.FACILITY_BOOKING_ID,
    U.NAME AS CustomerName,
    FB.FACILITY_NAME,
    FB.BOOKING_DATE,
    FB.GUESTS_COUNT,
    FB.TOTAL_PRICE,
    FB.BOOKING_STATUS,
    FB.BOOKING_TIME,
    U.EMAIL
FROM FACILITIES_BOOKING FB
JOIN USER_TABLE U ON FB.EMAIL = U.EMAIL
ORDER BY FB.BOOKING_DATE DESC
    6.REVENUE REPORT  
 SELECT
    CONCAT(DATENAME(MONTH, B.BOOKING_DATE), ' ', YEAR(B.BOOKING_DATE)) AS Month,
    SUM(DATEDIFF(DAY, B.CHECK_IN, B.CHECK_OUT) * R.PPN) AS Revenue,
    COUNT(*) AS Bookings
FROM BOOKINGS_TABLE B
JOIN ROOM_TABLE R ON B.ROOM_ID = R.ROOM_ID
WHERE B.BOOKING_DATE BETWEEN @StartDate AND @EndDate
GROUP BY YEAR(B.BOOKING_DATE), MONTH(B.BOOKING_DATE), DATENAME(MONTH, B.BOOKING_DATE)
ORDER BY YEAR(B.BOOKING_DATE), MONTH(B.BOOKING_DATE)
                7.BOOKING REPORT QUERY  
                                          SELECT 
B.BOOKING_ID,
U.NAME AS CustomerName,
B.ROOM_ID AS RoomNumber,
B.CHECK_IN,
B.CHECK_OUT,
B.BOOKING_DATE,
B.BOOKING_STATUS,
DATEDIFF(DAY, B.CHECK_IN, B.CHECK_OUT) * R.PPN AS TotalAmount,
B.EMAIL AS CustomerEmail
FROM BOOKINGS_TABLE B
JOIN USER_TABLE U ON B.EMAIL = U.EMAIL
JOIN ROOM_TABLE R ON B.ROOM_ID = R.ROOM_ID
WHERE B.BOOKING_DATE BETWEEN @FromDate AND @ToDate
ORDER BY B.BOOKING_DATE DESC
   

User  Authentication query :
SELECT PASSWORD, USER_TYPE
FROM USER_TABLE
WHERE EMAIL = @Email
CUSTOMER  :

REGISTRATION FORM :
         1.MAIN INSERT QUERY :
INSERT INTO USER_TABLE (NAME, PHONE_NUMBER, EMAIL, PASSWORD, USER_TYPE)
VALUES (@Name, @Phone, @Email, @Password, 'CUSTOMER')

         2.CHECK EMAIL EXISTENCE(BEFORE REGISTRATION)
                            SELECT COUNT(*) FROM USER_TABLE WHERE EMAIL = @Email
        3.GET USER DETAILS AFTER REGISTRATION FORM
SELECT USER_ID, NAME, EMAIL, USER_TYPE FROM USER_TABLE WHERE EMAIL = @Email
         4.GET ROOM DETAILS :
SELECT TYPE, PPN, DESCRIPTION, PICTURE, COMPLIMENTARY, FEATURES
FROM ROOM_TABLE
WHERE ROOM_ID = @RoomId
         5.GET ADDITIONAL ROOM P;ICTURES :
SELECT IMAGE_1, IMAGE_2, IMAGE_3
FROM ADDITIONAL_PICTURES
WHERE ROOM_ID = @RoomId
          6.GET USER INFORMATION
SELECT NAME
FROM USER_TABLE
WHERE EMAIL = @Email
           7.GET ALL ROOMS
SELECT ROOM_ID, TYPE, PPN, DESCRIPTION, PICTURE FROM ROOM_TABLE
           8.CHECK ROOM AVAILABILITY 
SELECT COUNT(*) 
FROM BOOKINGS_TABLE 
WHERE ROOM_ID = @RoomId 
AND BOOKING_STATUS IN ('PAID', 'BOOKED', 'CHECK-IN', 'PENDING', 'Confirmed', 'Checked-In')
AND (
    (CHECK_IN <= @CheckOut AND CHECK_OUT >= @CheckIn)
)

9.GET USER INFORMATION 
                  SELECT NAME FROM USER_TABLE WHERE EMAIL = @Email
10.SEARCH ROOM BY TYPE 
SELECT ROOM_ID, TYPE, PPN, DESCRIPTION, PICTURE
FROM ROOM_TABLE
WHERE TYPE LIKE @SearchText

11.GET ROOM PRICE (PPN)
               SELECT PPN FROM ROOM_TABLE WHERE ROOM_ID = @RoomId
12.UPDATE BOOKING STATUS TO PAID
                UPDATE BOOKINGS_TABLE SET BOOKING_STATUS='PAID' WHERE BOOKING_ID = @BookingId
13.INSERT PAYMENT METHOD 
                    INSERT INTO PAYMENT (P_ID, BOOKING_ID, CARD_NUMBER, EXP_DATE, CODE, BILL) 
VALUES (@P_ID, @BOOKING_ID, @CARD_NUMBER, @EXP_DATE, @CODE, @BILL)
 14.GET NEXT PAYMENT METHOD ID
               SELECT ISNULL(MAX(P_ID), 0) + 1 FROM PAYMENT
15.FIND ROOM ID BOOKING 
                SELECT ROOM_ID FROM BOOKINGS_TABLE WHERE BOOKING_ID = @BookingId

STAFF:

1.GET USER INFORMATION
              SELECT NAME FROM USER_TABLE WHERE EMAIL = @email
2.LOAD ROOM ASSIGHNMENTS(ALL STAFF)
SELECT RA.ASSIGNMENT_ID, RA.ROOM_ID, RA.ASSIGNED_DATE, RA.EMAIL, RA.ROOM_STATUS, UT.NAME
FROM ROOM_ASSIGNMENTS RA
JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
WHERE (@nameFilter = '' OR UT.NAME LIKE @nameFilter)
3.SEARCH ROOM ASSIGHNMENTS BY NAME 
SELECT RA.ASSIGNMENT_ID, RA.ROOM_ID, RA.ASSIGNED_DATE, RA.EMAIL, RA.ROOM_STATUS, UT.NAME
FROM ROOM_ASSIGNMENTS RA
JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
WHERE UT.NAME LIKE @nameFilter
4.INSERT LAVE REQUEST :
            INSERT INTO LEAVE_REQUEST (LEAVE_ID, EMAIL, FROM_DATE, TO_DATE, REASON, STATUS) 
VALUES (@Id, @Email, @FromDate, @ToDate, @Reason, 'Pending')
5.LOAD LEAVE HISTORY:
              SELECT RA.FROM_DATE, RA.TO_DATE, RA.REASON, RA.STATUS, UT.NAME 
FROM LEAVE_REQUEST RA
JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
WHERE RA.EMAIL = @Email
ORDER BY RA.FROM_DATE DESC
6.LOAD MY TASKS ;
           SELECT ROOM_ID, ROOM_STATUS, ASSIGNED_DATE
FROM ROOM_ASSIGNMENTS  
WHERE EMAIL = @myEmail 
ORDER BY ASSIGNED_DATE DESC
7.UPDATE TASKS STATUS TO COMPLETED ;
UPDATE ROOM_ASSIGNMENTS
SET ROOM_STATUS = 'CLEANED'
WHERE ROOM_ID = @roomId AND EMAIL = @email
8.VERIFY TASK OWNERSHIP:
SELECT COUNT(*)
FROM ROOM_ASSIGNMENTS
WHERE ROOM_ID = @roomId AND EMAIL = @email


9.SUBMIT MAINTENANCE REQUEST :
        INSERT INTO MaintenanceRequests (M_ID, ROOM_ID, PROBLEM, STATUS, SUBMIT_DATE, EMAIL) 
VALUES (@Id, @RoomId, @Problem, 'Pending', @Today, @Email)

10.GET NEXT MAINTENANCE REQUEST ID:
SELECT ISNULL(MAX(CAST(SUBSTRING(M_ID, 3, LEN(M_ID)) AS INT)), 0) + 1
FROM MaintenanceRequests


RECEPTIONIST :
 
1.GET RECEPTIONIST INFORMATION
               SELECT NAME FROM USER_TABLE WHERE EMAIL = @Email
2.LOAD BOOKING WITH FILTERING
SELECT RA.BOOKING_ID, RA.EMAIL, RA.ROOM_ID, RA.CHECK_IN, RA.CHECK_OUT,
RA.BOOKING_STATUS, UT.NAME
FROM BOOKINGS_TABLE RA
JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
WHERE 1=1
3.CHECK OUT GUEST:
              UPDATE BOOKINGS_TABLE 
SET BOOKING_STATUS = 'CHECK-OUT' 
WHERE BOOKING_ID = @BookingId

ADDITIONAL QUERY:
  1.GET STAFF EMAILS 
           SELECT EMAIL FROM USER_TABLE WHERE USER_TYPE = 'STAFF'
  2.GENERATE NEW ASSIGHNMENTS ID 
  SELECT ISNULL(MAX(CAST(ASSIGNMENT_ID AS INT)), 0) + 1 
FROM ROOM_ASSIGNMENTS 
WHERE ISNUMERIC(ASSIGNMENT_ID) = 1
3.INSERT ROOM ASSIGHNMENTS :
       INSERT INTO ROOM_ASSIGNMENTS 
(ASSIGNMENT_ID, ROOM_ID, ASSIGNED_DATE, EMAIL, ROOM_STATUS) 
VALUES (@id, @room, @date, @email, @status)
4.LEAVE STATISTICS
SELECT
COUNT(*) as TotalRequests,
SUM(CASE WHEN STATUS = 'APPROVED' THEN 1 ELSE 0 END) as ApprovedRequests,
SUM(CASE WHEN STATUS = 'PENDING' THEN 1 ELSE 0 END) as PendingRequests,
SUM(CASE WHEN STATUS = 'REJECTED' THEN 1 ELSE 0 END) as RejectedRequests,
SUM(CASE WHEN STATUS = 'APPROVED' THEN DATEDIFF(day, FROM_DATE, TO_DATE) + 1 ELSE 0 END) as TotalLeaveDays
FROM LEAVE_REQUEST
WHERE EMAIL = @Email

5.GET USER VERIFICATION DATA :
CREATE PROCEDURE sp_GetUserVerificationData
@Email NVARCHAR(255)
AS
BEGIN
SELECT
PHONE_NUMBER,
USER_ID,
SECURITY_QUESTION -- Optional: Add for additional security
FROM USER_TABLE
WHERE EMAIL = @Email
END
6.UPDATE PASSWORD
CREATE PROCEDURE sp_GetUserVerificationData
@Email NVARCHAR(255)
AS
BEGIN
SELECT
PHONE_NUMBER,
USER_ID,
SECURITY_QUESTION -- Optional: Add for additional security
FROM USER_TABLE
WHERE EMAIL = @Email
END


   Home Page:
   <img width="975" height="421" alt="image" src="https://github.com/user-attachments/assets/5ab30aa5-8b6d-42b8-a939-fe55c22e22fc" />
   <img width="975" height="255" alt="image" src="https://github.com/user-attachments/assets/6d1df9f7-cdcf-46f7-be1e-fb2da128310d" />
   <img width="975" height="454" alt="image" src="https://github.com/user-attachments/assets/0e0d09da-45e0-488f-baa7-d771ad5b8719" />

Login Page :
<img width="896" height="375" alt="image" src="https://github.com/user-attachments/assets/2b8fd5bf-a1e9-4e5c-b86c-7fb5694f9dcd" />

Signup Page/Registration  :
<img width="905" height="326" alt="image" src="https://github.com/user-attachments/assets/5d36299c-d036-4fb2-a30e-66264bcd8aec" />


Forgot Password Recovery Page :

<img width="895" height="460" alt="image" src="https://github.com/user-attachments/assets/657f4967-c888-4312-90e2-2ec3c8fafd47" />

ADMIN DASHBOARD :;

<img width="975" height="563" alt="image" src="https://github.com/user-attachments/assets/310c7cad-4168-4a1c-b288-3b56134dbab3" />

Leave Request Form:

<img width="975" height="557" alt="image" src="https://github.com/user-attachments/assets/ed911c42-79d6-4146-8e51-850b9bd9706f" />

Manage Room Form :

<img width="975" height="514" alt="image" src="https://github.com/user-attachments/assets/374b1a8e-ebd8-4799-bcef-b9df4903d781" />

User Management Form :

<img width="975" height="592" alt="image" src="https://github.com/user-attachments/assets/68f94de2-c0a4-45d3-89d6-94d866c61c9e" />

User Management Form (Assighn New Staff):

<img width="949" height="449" alt="image" src="https://github.com/user-attachments/assets/519c155a-1de9-4145-8fad-7d0259203131" />

User Management Form (Salary Management):

<img width="1040" height="728" alt="image" src="https://github.com/user-attachments/assets/f44500cb-a81b-4ec5-8c73-6036670a94aa" />

User Management Form (View Staff Details):

<img width="976" height="553" alt="image" src="https://github.com/user-attachments/assets/f4a2bc7e-9c1b-429e-8cf8-ca5d375e46da" />

Revenue(Daily):

<img width="874" height="381" alt="image" src="https://github.com/user-attachments/assets/0bb2e278-8932-4d5a-b881-3275e09f6b74" />

Reports :

<img width="975" height="630" alt="image" src="https://github.com/user-attachments/assets/eed3c94c-123e-4105-b547-26f311050318" />

CUSTOMER PANEL(ROOMS)  :

<img width="975" height="579" alt="image" src="https://github.com/user-attachments/assets/341ed5f5-ae05-4981-86e0-a1a87b866818" />

Rooms(Sorting):

<img width="975" height="641" alt="image" src="https://github.com/user-attachments/assets/974bf07f-b645-4d05-b1d8-62e8e3898adc" />

Booking Page:


<img width="975" height="623" alt="image" src="https://github.com/user-attachments/assets/78ab611a-c97a-4418-8ee7-6925cc772300" />

Multiple Payment Method :

<img width="975" height="358" alt="image" src="https://github.com/user-attachments/assets/27d70082-7ddf-4675-9ee6-8dd90deb3bfd" />


<img width="975" height="391" alt="image" src="https://github.com/user-attachments/assets/2a464410-53b6-4fdb-95a5-a930e3c60a20" />


<img width="975" height="404" alt="image" src="https://github.com/user-attachments/assets/2c86ab59-b527-4457-86a4-402b2c21110b" />


Print Receipt(Payment Receipt):

<img width="975" height="701" alt="image" src="https://github.com/user-attachments/assets/610e0df6-f8c7-474d-811c-8392e03d4a14" />

User Profile Dashboard  & Booking History:


<img width="975" height="523" alt="image" src="https://github.com/user-attachments/assets/2024006c-6b17-42fd-8a4c-2fa9adae12ef" />


Facilities Add Option :

<img width="975" height="504" alt="image" src="https://github.com/user-attachments/assets/eeb05e3e-a5e2-4198-8d03-4918592e7315" />

RECEPTIONIST DASHBOARD :

<img width="940" height="475" alt="image" src="https://github.com/user-attachments/assets/f0771e5d-11c8-47e9-8dfb-63e1b551d73c" />

Receptionist  Leave Form:

<img width="940" height="806" alt="image" src="https://github.com/user-attachments/assets/c4dbd9d2-856c-4d23-aeea-a7a2fc0f20b2" />

Receptionist Profile(Additional Info ):


<img width="940" height="625" alt="image" src="https://github.com/user-attachments/assets/ea9c5746-fa29-4611-86f5-c4e3c9bb5fde" />


<img width="940" height="602" alt="image" src="https://github.com/user-attachments/assets/29f867eb-815b-4a54-a0e4-722dc8c077f3" />


<img width="940" height="539" alt="image" src="https://github.com/user-attachments/assets/194413f3-9d5b-490d-9f92-83dc7031732a" />


Staff Dashboard :


<img width="940" height="525" alt="image" src="https://github.com/user-attachments/assets/77f62207-47a7-488d-8c04-f34403fe0938" />


<img width="940" height="566" alt="image" src="https://github.com/user-attachments/assets/5c7044a0-f42f-4b3e-959e-871a81557446" />


<img width="940" height="579" alt="image" src="https://github.com/user-attachments/assets/ff551119-215d-4908-b94e-38df264daad4" />


<img width="940" height="618" alt="image" src="https://github.com/user-attachments/assets/73a6ce8f-2fc3-44f7-96c9-e308a97a5c1d" />


<img width="940" height="627" alt="image" src="https://github.com/user-attachments/assets/9a3bf392-af47-4cf7-bc82-201b1d93eaff" />

























