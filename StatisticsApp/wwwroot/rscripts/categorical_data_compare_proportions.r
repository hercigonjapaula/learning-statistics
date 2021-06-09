library(datasets)
library(graphics)
library(ggplot2)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/"

## Skup podataka
data <- read.table(as.character(args[1]),sep=";",header = TRUE)
variable1 <- as.numeric(args[2])
variable2 <- as.numeric(args[3])
level1 <- as.character(args[4])
level2 <- as.character(args[5])
alternative.hypothesis <- as.character(args[6])

## Test o dvije proporcije
# broj eksperimenata iz oba uzorka
n = c(length(data[,variable1][data[,variable2] == level2]), length(df$default[df$gender=="m"]))

# broj uspjeha iz oba uzorka
x = c(length(which(df$default[df$gender=="f"]=="delinquent")),length(which(df$default[df$gender=="m"]=="delinquent")))

# test o dvije proporcije (hi-kvadrat umjesto z-testa) - bez tzv. Yatesove korekcije
prop.test(x,n,alternative="t",correct="FALSE")