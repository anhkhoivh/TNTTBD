We will need to build Angular frontend project Project UI: TNTTBD.UI
The UI client app will be placed in TNTTBD, besides TNTTBD.Services, therefore, based on Services and APIs to create the pages.  I will be at Ask mode first so you can ask until we have enough information, then I will switch to Plan mode to have a thorough design before Agent. Note that you can take note right into file ui-create-details.md and I will review to make sure my requirements are on track. No coding now.

APIs haven’t been completed yet, but will assume we will have those APIs (or you plan to).
Technical requirements: Angular newest version so we can discuss to decide what state management to use (Signal + Service or rxjs + Service or ngrx) ; using components of Angular Material, and should be used in PC web and mobile web so it should be responsive.

App manages Eucharistic Youth Movement in a Parish ( Đoàn Thiếu nhi Thánh Thể trong 1 giáo xứ), you will need to examine my TNTTBD.Services first: 
    EucharisticYouth (or simply Youth) info
        Spiritual Milestones
        Eucharistic Youth photos
    Contact
        Join entity with Youth: EucharisticYouthContact
    Catechist info
        Catechist photos
    User: users of the app, could be linked to Catechist, Youth or Contact; have UserRole to grant permission to specific views or different behaviors
        User External Login: single sign on using Google or Microsoft Authenticator 
    Catechism Year
        Catechism Class
            Catechism Sessions of the class
                Catechism Session Attendances: Youth attendance of a class session
            Exam of the class 
                Exam Results: results of each student in the exam
            Join entity: 
                CatechismClassEucharisticYouth: enrollment of EucharisticYouths
                CatechismClassCatechist enrollment of Catechists into a class, have a specific role here


