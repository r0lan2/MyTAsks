create table ApplicationSettings
(
   SettingId						int not null auto_increment,
   ConfigurationKey                 varchar(300),
   ConfigurationValue               varchar(300),
   DefaultValue						varchar(300),
   Description						varchar(500),
   LastUpdatedTime					date,
   primary key (SettingId)
);