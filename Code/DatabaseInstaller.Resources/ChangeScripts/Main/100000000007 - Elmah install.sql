CREATE TABLE IF NOT EXISTS `elmah_error` (
  `ErrorId` CHAR(36) NOT NULL ,
  `Application` VARCHAR(60) NOT NULL ,
  `Host` VARCHAR(50) NOT NULL ,
  `Type` VARCHAR(100) NOT NULL ,
  `Source` VARCHAR(60) NOT NULL ,
  `Message` VARCHAR(500) NOT NULL ,
  `User` VARCHAR(50) NOT NULL ,
  `StatusCode` INT(10) NOT NULL ,
  `TimeUtc` DATETIME NOT NULL ,
  `Sequence` INT(10) NOT NULL AUTO_INCREMENT ,
  `AllXml` TEXT NOT NULL ,
  PRIMARY KEY (`Sequence`) ,
  UNIQUE INDEX `IX_ErrorId` (`ErrorId`(8) ASC) ,
  INDEX `IX_ELMAH_Error_App_Time_Seql` (`Application`(10) ASC, `TimeUtc` DESC, `Sequence` DESC) ,
  INDEX `IX_ErrorId_App` (`ErrorId`(8) ASC, `Application`(10) ASC) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8
CHECKSUM = 1
DELAY_KEY_WRITE = 1
ROW_FORMAT = DYNAMIC;

