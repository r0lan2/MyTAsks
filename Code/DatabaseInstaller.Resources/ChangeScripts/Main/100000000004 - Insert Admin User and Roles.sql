
INSERT INTO users (`Id`,`Email`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEndDateUtc`,`LockoutEnabled`,`AccessFailedCount`,`UserName`,`FullName`,`Address`,`DNI`,`Language`) 
VALUES ('f054418a-2bee-49b1-89bd-7387f69f95f0','admin@mytasks.cl',1,'ADggy6naWtPgOdgAWhwGKQj1EUtBWuRkpYXnUpYXN5+ac2D+2AieripppwYT4jiKIA==','78b9c51e-a6ff-47d3-b54b-84ab7223b157',null,0,0,'2016-03-18 16:07:24',0,0,'admin','Mr BigLamp','','156508810','en-us');

INSERT INTO users (`Id`,`Email`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEndDateUtc`,`LockoutEnabled`,`AccessFailedCount`,`UserName`,`FullName`,`Address`,`DNI`,`Language`) 
VALUES ('6e182b00-ba83-4a29-8cc8-700c440b5a36','rolandomartinezg@mytasks.cl',1,'ADggy6naWtPgOdgAWhwGKQj1EUtBWuRkpYXnUpYXN5+ac2D+2AieripppwYT4jiKIA==','78b9c51e-a6ff-47d3-b54b-84ab7223b157',null,0,0,'2016-03-18 16:07:24',0,0,'rolandomartinezg','Rolando Martinez Gallardo','','152958587','en-us');

INSERT INTO users (`Id`,`Email`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEndDateUtc`,`LockoutEnabled`,`AccessFailedCount`,`UserName`,`FullName`,`Address`,`DNI`,`Language`) 
VALUES ('bf3b47ba-ab83-48b3-8537-0fa642487b9e','rolado_martinez@mytasks.cl',1,'ALdOzk4o80Oy07YL5tgQjLhkDWq1UpCGrAEDyXGhHVlJeUjMCwnADc3KAZU11KZqPA==','78b9c51e-a6ff-47d3-b54b-84ab7223b157',null,0,0,'2016-03-18 16:07:24',0,0,'agallardo','Alberto Gallardo','','156508810','en-us');

INSERT INTO roles (`Id`,`Name`) VALUES ('0e908ca4-2064-4a66-9bd7-5ce623ef19c6','ProjectManager');
INSERT INTO roles (`Id`,`Name`) VALUES ('8a23e6e9-6939-4061-ac86-8e393e7d35ab','Admin');
INSERT INTO roles (`Id`,`Name`) VALUES ('2266bcb5-42ba-4af7-bd42-cf000c3a0602','Developer');
INSERT INTO roles (`Id`,`Name`) VALUES ('2acd9943-904e-4d0b-b8a7-2ded83c62be6','User');


INSERT INTO userroles (`UserId`,`RoleId`) VALUES ('f054418a-2bee-49b1-89bd-7387f69f95f0','8a23e6e9-6939-4061-ac86-8e393e7d35ab');
INSERT INTO userroles (`UserId`,`RoleId`) VALUES ('6e182b00-ba83-4a29-8cc8-700c440b5a36','2266bcb5-42ba-4af7-bd42-cf000c3a0602');
INSERT INTO userroles (`UserId`,`RoleId`) VALUES ('bf3b47ba-ab83-48b3-8537-0fa642487b9e','0e908ca4-2064-4a66-9bd7-5ce623ef19c6');








