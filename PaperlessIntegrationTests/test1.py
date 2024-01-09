import requests

BASE_URL = 'http://localhost:50352/api/'


def test_sample_correspondents():
    response = requests.get(BASE_URL + 'correspondents')

    assert response.status_code == 200

    response_data = response.json()

    assert len(response_data) == 5


if __name__ == '__main__':
    test_sample_correspondents()