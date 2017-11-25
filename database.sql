-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema FoxyFace
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema FoxyFace
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `FoxyFace` DEFAULT CHARACTER SET utf8 ;
USE `FoxyFace` ;

-- -----------------------------------------------------
-- Table `FoxyFace`.`User`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `FoxyFace`.`User` (
  `Users_id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Users_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `FoxyFace`.`Post`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `FoxyFace`.`Post` (
  `Post_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  `title` VARCHAR(45) NOT NULL,
  `description` VARCHAR(45) NOT NULL,
  `path` VARCHAR(45) NOT NULL,
  `date` DATETIME NOT NULL DEFAULT NOW(),
  PRIMARY KEY (`Post_id`),
  INDEX `fk_Post_1_idx` (`user_id` ASC),
  CONSTRAINT `fk_post_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `FoxyFace`.`User` (`Users_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `FoxyFace`.`Rating`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `FoxyFace`.`Rating` (
  `Rating_id` INT NOT NULL,
  `post_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  `stars` INT NOT NULL,
  PRIMARY KEY (`Rating_id`, `user_id`),
  INDEX `fk_Rating_post_id_idx` (`post_id` ASC),
  INDEX `fk_Rating_user_id_idx` (`user_id` ASC),
  CONSTRAINT `fk_Rating_post_id`
    FOREIGN KEY (`post_id`)
    REFERENCES `FoxyFace`.`Post` (`Post_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Rating_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `FoxyFace`.`User` (`Users_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `FoxyFace`.`Comment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `FoxyFace`.`Comment` (
  `Comment_id` INT NOT NULL,
  `post_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  `text` TEXT NOT NULL,
  `date` DATETIME NOT NULL DEFAULT NOW(),
  PRIMARY KEY (`Comment_id`),
  INDEX `fk_Comment_user_id_idx` (`user_id` ASC),
  INDEX `fk_Comment_post_id_idx` (`post_id` ASC),
  CONSTRAINT `fk_Comment_post_id`
    FOREIGN KEY (`post_id`)
    REFERENCES `FoxyFace`.`Post` (`Post_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Comment_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `FoxyFace`.`User` (`Users_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
