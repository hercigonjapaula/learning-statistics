library(datasets)
library(graphics)
library(ggplot2)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = args[1]

## Skup podataka
data <- read.table(as.character(args[2]),sep=";",header = TRUE)
variable1 <- as.numeric(args[3])
variable2 <- as.numeric(args[4])
alternative.hypothesis <- as.character(args[5])

## Test o dvije proporcije
# kontingencijska tablica
tbl = table(data[,variable1],data[,variable2])
# hi-kvadrat test nezavisnosti
chisq.test(tbl,correct=F)
# fisher-irwinov egzaktni test
fisher.test(tbl,alternative = alternative.hypothesis) 