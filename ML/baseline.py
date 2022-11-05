import pandas as pd
import numpy as np
from datetime import date, timedelta
from collections import OrderedDict
import json

def get_dates_from_period(period: str) -> list:
    #example of period '05/2021-11/2021'
    start_date = (period.split('-')[0])
    last_date = (period.split('-')[1])
    cur = date(int(start_date.split('/')[1]), int(start_date.split('/')[0]), 1)
    yesterday = date(int(last_date.split('/')[1]), int(last_date.split('/')[0]), 1)
    delta = timedelta(days=1)
    dates = []
    while cur <= yesterday:
        new_date = cur.strftime('%m/%Y')[0:3] + cur.strftime('%m/%Y')[3:]
        dates.append(new_date)
        cur += delta
    unique = []
    for date_norm in dates:
        if date_norm not in unique: unique.append(date_norm)
    return unique
    
def ranking_data(whole_data: pd.DataFrame, region: str, period: str) -> OrderedDict: 
    whole_data = whole_data.loc[(whole_data['napr'] == 'ИМ') & (whole_data['period'].isin(get_dates_from_period(period))) & (whole_data['Region'] == region)]
    unique_tnved = np.array(whole_data['tnved_6'].unique())
    stoims = dict()

    for i in range(len(unique_tnved)):
        stoims[unique_tnved[i]] = sum(whole_data.loc[whole_data['tnved_6'] == unique_tnved[i]]['Stoim'])

    best_stoims = OrderedDict(sorted(stoims.items(), key=lambda x: -x[1]))
    return best_stoims 


def main():
    with open('params.json', encoding='utf-8') as f:
        params = json.load(f)
    region = params['region']
    period = params['period']
    whole_data = pd.read_csv("whole_data.csv", decimal = ',', low_memory=False)
    print(ranking_data(whole_data, region, period))    

if __name__ == '__main__':
    main()