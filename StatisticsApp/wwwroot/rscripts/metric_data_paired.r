library(datasets)
library(graphics)
library(ggplot2)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
path = "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/"

## Skup podataka
data <- read.table(as.character(args[1]), header = TRUE)
alternative.hypothesis <- as.character(args[2])
confidence.interval <- as.numeric(args[3])

## Histogrami
png(file=paste(path, "test_plots/histogram1.png", sep = ""))
hist(data[,1], main = paste(names(data[,1]), "histogram", sep = " "))
dev.off()
png(file=paste(path, "test_plots/histogram2.png", sep = ""))
hist(data[,2], main = paste(names(data[,2]), "histogram", sep = " "))
dev.off()

## Test za uparene podatke
t.test(data[,1], data[,2], paired = TRUE, 
       alternative = alternative.hypothesis)