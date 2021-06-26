library(datasets)
library(graphics)
library(ggplot2)
library(gginference)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = args[1]

## Skup podataka
data <- read.table(as.character(args[2]),sep=",",header = TRUE)
variable1 <- as.numeric(args[3])
variable2 <- as.numeric(args[4])
alternative.hypothesis <- as.character(args[5])

## Test o dvije proporcije
# kontingencijska tablica
tbl = table(data[,variable1],data[,variable2])
sink(paste(path, "contingency_table.txt", sep = ""))
print(tbl)
sink()
# hi-kvadrat test nezavisnosti
test.result <- chisq.test(tbl,correct=F)
cat(test.result$statistic, test.result$parameter, 
    test.result$p.value, sep = " ")
ggchisqtest(test.result, colaccept="lightsteelblue1", 
            colreject="grey84", colstat="navyblue")
ggsave("test_plot.png", path = paste(path, "test_plots", sep = ""))