/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     22-11-2015 23:30:30                          */
/*==============================================================*/


drop table if exists Category;

drop table if exists Area;

drop table if exists Customer;

drop table if exists Priority;

drop table if exists Project;

drop table if exists Ticket;

drop table if exists TicketStatus;

drop table if exists WorkingOn;

/*==============================================================*/
/* Table: Category                                              */
/*==============================================================*/
create table Category
(
   CategoryId           int not null auto_increment,
   Name                 varchar(300),
   primary key (CategoryId)
);

/*==============================================================*/
/* Table: Area                                              */
/*==============================================================*/
create table Area
(
   AreaId           int not null auto_increment,
   Name                 varchar(300),
   ProjectId		int,
   primary key (AreaId)
);

/*==============================================================*/
/* Table: Customer                                              */
/*==============================================================*/
create table Customer
(
   CustomerId           int not null auto_increment,
   Name                 varchar(500),
   MainContactName		varchar(500),
   MainContactEmail		varchar(500),
   primary key (CustomerId)
);

/*==============================================================*/
/* Table: Priority                                              */
/*==============================================================*/
create table Priority
(
   PriorityId           int not null auto_increment,
   Name                 varchar(300),
   Code					varchar(20),
   primary key (PriorityId)
);

/*==============================================================*/
/* Table: Project                                               */
/*==============================================================*/
create table Project
(
   ProjectId            int not null auto_increment,
   CustomerId           int,
   ProjectManagerId     varchar(128),
   ProjectName          varchar(200),
   Description          text,
   primary key (ProjectId)
);

/*==============================================================*/
/* Table: Ticket                                                */
/*==============================================================*/
create table Ticket
(
   TicketDetailId             int not null auto_increment,   
   TicketNumber			int,
   ProjectId            int,
   AreaId		        int,
   PriorityId           int,
   CategoryId           int,
   TicketStatusId       int,
   Title                varchar(800),
   Details              text,
   IsHtml               bool,
   CreatedBy            varchar(200),
   CreatedDate          date,
   OwnerUserId          varchar(200),
   AssignedTo           varchar(200),
   LastUpdateBy         varchar(200),
   LastUpdateDate       date,   
   HasFiles             bool,
   ParentTicketId		int,
   EditionMarkAsDeleted bool,
   IsLastDetail bool,
   IsBillable bool,
   primary key (TicketDetailId)
);

/*==============================================================*/
/* Table: File                                          */
/*==============================================================*/
create table FileData
(
   FileId       int not null auto_increment,
   FileName                 varchar(200),
   Extension                varchar(20),
   Path						varchar(400),
   FileGuid					varchar(100),   
   TicketDetailId					int null,
   primary key (FileId)
);



/*==============================================================*/
/* Table: TicketStatus                                          */
/*==============================================================*/
create table TicketStatus
(
   TicketStatusId       int not null auto_increment,
   Name                 varchar(80),
   primary key (TicketStatusId)
);

/*==============================================================*/
/* Table: WorkingOn                                             */
/*==============================================================*/
create table WorkingOn
(
   WorkingOnId          int not null auto_increment,
   TicketDetailId             int,
   UserId               int,
   StartDate            date,
   EndDate              date,
   StartTime            time,
   EndTime              time,
   primary key (WorkingOnId)
);

alter table Project add constraint FK_REFERENCE_1 foreign key (CustomerId)
      references Customer (CustomerId) on delete restrict on update restrict;

alter table Ticket add constraint FK_REFERENCE_2 foreign key (ProjectId)
      references Project (ProjectId) on delete restrict on update restrict;

alter table WorkingOn add constraint FK_REFERENCE_3 foreign key (TicketDetailId)
      references Ticket (TicketDetailId) on delete restrict on update restrict;

alter table Ticket add constraint FK_REFERENCE_4 foreign key (AreaId)
      references Area (AreaId) on delete restrict on update restrict;

alter table Ticket add constraint FK_REFERENCE_5 foreign key (PriorityId)
      references Priority (PriorityId) on delete restrict on update restrict;

alter table Ticket add constraint FK_REFERENCE_6 foreign key (CategoryId)
      references Category (CategoryId) on delete restrict on update restrict;

alter table Ticket add constraint FK_REFERENCE_7 foreign key (TicketStatusId)
      references TicketStatus (TicketStatusId) on delete restrict on update restrict;
	  	  	  
alter table Area add constraint FK_REFERENCE_8 foreign key (ProjectId)
      references Project (ProjectId) on delete restrict on update restrict;

