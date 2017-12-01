USE foxyface;

-- -----------------------------------------------------
-- Table `foxyface`.`user`
-- -----------------------------------------------------

CREATE TABLE IF NOT EXISTS user (
  `User_id` INT(11) NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(128) NOT NULL,
  `email` VARCHAR(128) NOT NULL,
  `salt` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`User_id`))
  ENGINE = InnoDB
  DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `foxyface`.`post`
-- -----------------------------------------------------

CREATE TABLE IF NOT EXISTS post (
  `Post_id` INT(11) NOT NULL AUTO_INCREMENT,
  `user_id` INT(11) NOT NULL,
  `title` VARCHAR(128) NOT NULL,
  `description` VARCHAR(255) NOT NULL,
  `path` VARCHAR(255) NOT NULL,
  `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Post_id`),
  INDEX `fk_Post_1_idx` (`user_id` ASC),
  CONSTRAINT `fk_post_user_id`
  FOREIGN KEY (`user_id`)
  REFERENCES `foxyface`.`user` (User_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
  ENGINE = InnoDB
  DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `foxyface`.`comment`
-- -----------------------------------------------------

CREATE TABLE IF NOT EXISTS comment (
  `Comment_id` INT(11) NOT NULL AUTO_INCREMENT,
  `post_id` INT(11) NOT NULL,
  `user_id` INT(11) NOT NULL,
  `text` TEXT NOT NULL,
  `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Comment_id`),
  INDEX `fk_Comment_user_id_idx` (`user_id` ASC),
  INDEX `fk_Comment_post_id_idx` (`post_id` ASC),
  CONSTRAINT `fk_Comment_post_id`
  FOREIGN KEY (`post_id`)
  REFERENCES `foxyface`.`post` (`Post_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Comment_user_id`
  FOREIGN KEY (`user_id`)
  REFERENCES `foxyface`.`user` (User_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
  ENGINE = InnoDB
  DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `foxyface`.`rating`
-- -----------------------------------------------------

CREATE TABLE IF NOT EXISTS rating (
  `Rating_id` INT(11) NOT NULL AUTO_INCREMENT,
  `post_id` INT(11) NOT NULL,
  `user_id` INT(11) NOT NULL,
  `stars` INT(11) NOT NULL,
  PRIMARY KEY (`Rating_id`),
  INDEX `fk_Rating_post_id_idx` (`post_id` ASC),
  INDEX `fk_Rating_user_id_idx` (`user_id` ASC),
  CONSTRAINT `fk_Rating_post_id`
  FOREIGN KEY (`post_id`)
  REFERENCES `foxyface`.`post` (`Post_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Rating_user_id`
  FOREIGN KEY (`user_id`)
  REFERENCES `foxyface`.`user` (User_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
  ENGINE = InnoDB
  DEFAULT CHARACTER SET = utf8;

-- -----------------------------------------------------
-- Table `foxyface`.`session`
-- -----------------------------------------------------

CREATE TABLE IF NOT EXISTS `session` (
  `Session_id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `token` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Session_id`),
  INDEX `FK_session_user_id_idx` (`user_id` ASC),
  CONSTRAINT `FK_session_user_id`
  FOREIGN KEY (`user_id`)
  REFERENCES `foxyface`.`user` (`User_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
  ENGINE = InnoDB;
