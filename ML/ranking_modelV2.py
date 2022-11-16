import pandas as pd
import numpy as np
from datetime import date, timedelta
from collections import OrderedDict
import json
from sklearn.linear_model import LinearRegression
from sklearn.preprocessing import StandardScaler


def get_dates_from_period(period: str) -> list:
    # example of period '05/2021-11/2021'
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


def get_all_tnved_from_period(whole_data: pd.DataFrame, period: str) -> list:
    whole_data = whole_data.loc[(whole_data['period'].isin(get_dates_from_period(period)))]
    unique_tnved = list(whole_data['tnved_6'].unique())
    return unique_tnved


def make_score_for_one_month(whole_data: pd.DataFrame, period: str) -> OrderedDict:
    list_bad_countries = ['US', 'AU', 'AL', 'UK', 'CA', 'NO', 'KR', 'SG', 'TW', 'UA', 'CH', 'ME', 'JP', 'AT', 'BE',
                          'BG', 'HU', 'DE', 'GR', 'DK', 'ES', 'IT', 'LV', 'LT', 'NL', 'PL', 'PT', 'RO', 'SK', 'SL',
                          'FI', 'FR', 'HR', 'CZ', 'SE', 'EE']
    whole_data = whole_data.loc[(whole_data['period'] == period) & whole_data['nastranapr'].isin(list_bad_countries)]
    unique_tnved = np.array(whole_data['tnved_6'].unique())

    data_import = whole_data.loc[whole_data['napr'] == 'ИМ']
    data_export = whole_data.loc[whole_data['napr'] == 'ЭК']

    coof_for_tnved_import = dict()
    for i in range(len(unique_tnved)):  # get the stoim for all period for each import tnved
        coof_for_tnved_import[unique_tnved[i]] = sum(
            data_import.loc[data_import['tnved_6'] == unique_tnved[i]]['Stoim'])

    coof_for_tnved_export = dict()  # get the stoim for all period for each export tnved
    for i in range(len(unique_tnved)):
        coof_for_tnved_export[unique_tnved[i]] = sum(
            data_export.loc[data_export['tnved_6'] == unique_tnved[i]]['Stoim'])

    difference_ex_im = dict()
    for i in range(len(unique_tnved)):  # get the difference between stoim import and stoim export for each tnved
        difference_ex_im[unique_tnved[i]] = (
                coof_for_tnved_export[unique_tnved[i]] - coof_for_tnved_import[unique_tnved[i]])

    mean_difference = sum(list(difference_ex_im.values())) / len(list(difference_ex_im.values()))
    for i in range(len(unique_tnved)):  # calculating score
        if coof_for_tnved_export[unique_tnved[i]] - coof_for_tnved_import[unique_tnved[i]] != 0:
            coof_for_tnved_import[unique_tnved[i]] /= (coof_for_tnved_export[unique_tnved[i]] - coof_for_tnved_import[
                unique_tnved[i]]) / mean_difference

    coof_for_tnved_import = OrderedDict(sorted(coof_for_tnved_import.items(), key=lambda x: -x[1]))
    if difference_ex_im == 0:
        return unique_tnved[i]['Stoim']
    return coof_for_tnved_import


def get_next_month_from_list(period: list) -> list:
    # example of period : ['10/2021', '11/2021', '12/2021', '01/2021']
    # output:             [10, 11, 12, 13, 14]
    min_date = int(period[0].split('/')[0])
    incorrect_monthes = []
    for i in range(len(period)+1):
      incorrect_monthes.append(min_date + i)
    return incorrect_monthes



def recommendation(whole_data: pd.DataFrame, period: list) -> OrderedDict:
    # example of period : ['10/2021', '11/2021', '12/2021', '01/2021']
    all_tnved = get_all_tnved_from_period(whole_data, f'{period[0]}-{period[-1]}')
    coof_data = pd.DataFrame(data={'tnved': all_tnved})

    if len(period) >= 5:
        number_of_monthes = 5
    else:
        number_of_monthes = len(period)

    for month in range(number_of_monthes):
        scores = make_score_for_one_month(whole_data, period[month])
        new_month_dict = {
            'tnved': scores.keys(),
            period[month]: scores.values()
        }
        coof_data[period[month]] = pd.Series(scores.values())

    coof_data = coof_data.fillna(0)
    scaler = StandardScaler()
    coof_data[period] = scaler.fit_transform(coof_data[period])
    ranking_dict = dict()
    inccorect_month = get_next_month_from_list(period)
    x = []
    [x.append([float(inccorect_month[i])]) for i in range(number_of_monthes)]

    for i in range(coof_data.shape[0]):
        y = list(coof_data.iloc[i])[1:]
        tnved_number = list(coof_data.iloc[i])[0]
        lin_reg = LinearRegression()
        lin_reg.fit(x, y)
        pred = lin_reg.predict([[float(inccorect_month[number_of_monthes])]])  # 4
        ranking_dict[tnved_number] = pred[0]

    ranking_dict = OrderedDict(sorted(ranking_dict.items(), key=lambda x: -x[1]))
    return ranking_dict