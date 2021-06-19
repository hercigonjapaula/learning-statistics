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
level1 <- as.character(args[5])
level21 <- as.character(args[6])
level22 <- as.character(args[7])
alternative.hypothesis <- as.character(args[8])

## Test o dvije proporcije
# broj eksperimenata iz oba uzorka
n = c(length(data[,variable1][data[,variable2] == level21]), 
      length(data[,variable1][data[,variable2] == level22]))
# broj uspjeha iz oba uzorka
x = c(length(which(data[,variable1][data[,variable2] == level21] == level1)), 
      length(data[,variable1][data[,variable2] == level22] == level1))
# test o dvije proporcije
test.result <- prop.test(x, n, alternative = alternative.hypothesis, correct="FALSE")
cat(test.result$statistic, test.result$parameter, 
    test.result$p.value, test.result$conf.int, 
    test.result$estimate, sep = " ")
ggproptest(test.result, colaccept="lightsteelblue1", 
           colreject="grey84", colstat="navyblue")
ggsave("test_plot.png", path = path)