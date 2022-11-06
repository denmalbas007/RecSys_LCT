import pandas as pd
import numpy as np
from datetime import date, timedelta
from collections import OrderedDict
import json
from sklearn.linear_model import LinearRegression
from sklearn.preprocessing import StandardScaler


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
    

def get_all_tnved_from_period(whole_data: pd.DataFrame, region: str, period: str) -> list:
    whole_data = whole_data.loc[(whole_data['period'].isin(get_dates_from_period(period))) & (whole_data['Region'] == region)]
    unique_tnved = list(whole_data['tnved_6'].unique())
    return unique_tnved


def make_score_for_one_month(whole_data: pd.DataFrame, region: str, period: str) -> OrderedDict: 
    list_bad_countries = ['US', 'AU', 'AL', 'UK', 'CA', 'NO', 'KR', 'SG', 'TW', 'UA', 'CH', 'ME', 'JP', 'AT', 'BE', 'BG', 'HU', 'DE', 'GR', 'DK', 'ES', 'IT', 'LV', 'LT', 'NL', 'PL', 'PT', 'RO', 'SK', 'SL', 'FI', 'FR', 'HR', 'CZ', 'SE', 'EE']
    whole_data = whole_data.loc[(whole_data['period'] == period) & (whole_data['Region'] == region) & whole_data['nastranapr'].isin(list_bad_countries)]
    unique_tnved = np.array(whole_data['tnved_6'].unique())

    data_import = whole_data.loc[whole_data['napr'] == 'ИМ']
    data_export = whole_data.loc[whole_data['napr'] == 'ЭК']

    coof_for_tnved_import = dict()
    for i in range(len(unique_tnved)):    #get the stoim for all period for each import tnved
        coof_for_tnved_import[unique_tnved[i]] = sum(data_import.loc[data_import['tnved_6'] == unique_tnved[i]]['Stoim'])

    coof_for_tnved_export = dict()        #get the stoim for all period for each export tnved
    for i in range(len(unique_tnved)):
        coof_for_tnved_export[unique_tnved[i]] = sum(data_export.loc[data_export['tnved_6'] == unique_tnved[i]]['Stoim'])
    
    difference_ex_im = dict()
    for i in range(len(unique_tnved)):    #get the difference between stoim import and stoim export for each tnved
        difference_ex_im[unique_tnved[i]] = (coof_for_tnved_export[unique_tnved[i]]-coof_for_tnved_import[unique_tnved[i]])

    mean_difference = sum(list(difference_ex_im.values()))/len(list(difference_ex_im.values()))
    for i in range(len(unique_tnved)):    #calculating score
        if coof_for_tnved_export[unique_tnved[i]]-coof_for_tnved_import[unique_tnved[i]] != 0 and mean_difference != 0:
            coof_for_tnved_import[unique_tnved[i]]/=(coof_for_tnved_export[unique_tnved[i]]-coof_for_tnved_import[unique_tnved[i]])/mean_difference

    coof_for_tnved_import = OrderedDict(sorted(coof_for_tnved_import.items(), key=lambda x: -x[1]))
    if difference_ex_im == 0:
        return unique_tnved[i]['Stoim']
    return coof_for_tnved_import 


def get_next_month_from_list(period: list) -> list:
    #example of period : ['10/2021', '11/2021', '12/2021', '01/2021']
    #output:             [10, 11, 12, 13, 14]
    min_date = int(period[0].split('/')[0])
    min_year = int(period[0].split('/')[1])
    incorrect_monthes = [min_date, min_date+1, min_date+2, min_date+3, min_date+4]
    # if min_date == 12:   #[12, 13, 14, 15]
    #     incorrect_monthes = [min_date, min_date+1, min_date+2, min_date+3, min_date+4]
    # elif min_date+1 == 12: #[11, 12, 13, 14]
    #     incorrect_monthes = [min_date, min_date+1, min_date+2, min_date+3, min_date+4]
    # elif min_date+2 == 12: #[10, 11, 12, 13]
    #     incorrect_monthes = [min_date, min_date+1, min_date+2, min_date+3, min_date+4]
    # elif min_date+3 == 12: #[9, 10, 11, 12]
    return incorrect_monthes

def recommendation(whole_data: pd.DataFrame, region: str, period: list) -> OrderedDict:
    #example of period : ['10/2021', '11/2021', '12/2021', '01/2021']
    all_tnved = get_all_tnved_from_period(whole_data, region, f'{period[0]}-{period[-1]}')
    coof_data = pd.DataFrame(data={'tnved':all_tnved})
    for month in range(4):
        scores = make_score_for_one_month(whole_data, region, period[month])
        new_month_dict = {
            'tnved':        scores.keys(),
            period[month]:  scores.values()
        }
        coof_data[period[month]] = pd.Series(scores.values())
    coof_data = coof_data.fillna(0)
    scaler = StandardScaler()
    coof_data[period] = scaler.fit_transform(coof_data[period])
    ranking_dict = dict()
    inccorect_month = get_next_month_from_list(period)
    x = [[float(inccorect_month[0])], [float(inccorect_month[1])], [float(inccorect_month[2])], [float(inccorect_month[3])]]
    for i in range(coof_data.shape[0]):
        y = list(coof_data.iloc[i])[1:]
        tnved_number = list(coof_data.iloc[i])[0]
        lin_reg = LinearRegression()
        lin_reg.fit(x, y)
        pred = lin_reg.predict([[float(inccorect_month[4])]])
        ranking_dict[tnved_number] = pred[0]

    ranking_dict = OrderedDict(sorted(ranking_dict.items(), key=lambda x: -x[1]))
    return ranking_dict


def main():
    with open('params.json', encoding='utf-8') as f:
        params = json.load(f)
    region = params['region']
    whole_data = pd.read_csv("whole_data.csv", decimal = ',', low_memory=False) 
    
    ranking_dict = recommendation(whole_data, region, ['09/2021', '10/2021', '11/2021', '12/2021'])
    top_10 = 0
    for tnved in ranking_dict.keys():
        if top_10 != 3:
            print(tnved, " ", ranking_dict[tnved])
            top_10+=1
        else:
            break
    

if __name__ == '__main__':
    main()