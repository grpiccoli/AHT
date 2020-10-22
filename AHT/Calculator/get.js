'use strict';
const puppeteer = require('puppeteer');
const urlencode = require('urlencode');
let brw;

const end = async (msg, success = false) => {
    brw.close().then(_ => {
        if (success) {
            console.log(msg);
        } else {
            throw msg;
        }
    }).catch(_ => {
        if (success) {
            console.log(msg);
        } else {
            throw msg;
        }
    });
};

const initBrowser = async () => {
    await puppeteer.launch(
        {
            ignoreHTTPSErrors: true,
            headless: false,
            args: ['--no-sandbox', '--disable-setuid-sandbox']
        }
    ).then(async browser => {
        brw = browser;
        const page = (await brw.pages())[0];
        await page.goto('https://bonowebfon.fonasa.cl/', { timeout: 120000, waitUntil: 'networkidle2' }).catch(e => { throw e; });
    }).catch(e => { throw e; });
};

const readInfo = async data => {
    var page = (await brw.pages())[0];
    var user = await page.evaluate(async (rut) => {
        var url = urlAjax('bono','execWSCertifPagador');
        var body = `RutPagador=${rut}`;
        return await fetch(url, {
            method: 'POST',
            headers: new Headers({ 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }),
            body: body
        }).then(response => response.json())
        .then(j => `${j.extNombres} ${j.extApellidoPat} ${j.extApellidoMat}_${j.extFechaNacimi.substring(6,8)}-${j.extFechaNacimi.substring(4,6)}-${j.extFechaNacimi.substring(0,4)}`)
		.catch(e => { throw e; });
    }, data).catch(e => { throw e; });
	return user;
};

async function asyncForEach(array, callback) {
  for (let index = 0; index < array.length; index++) {
    await callback(array[index], index, array);
  }
}

(async() =>
{
	const args = process.argv.slice(2);
	    await initBrowser()
		.then(async () => {
			var results = [];
			await asyncForEach(args, async (rut) => {
				var r = await readInfo(rut);
				results.push(r);
			});
			return urlencode(results.join('~'));
		})
		.then(r => end(r, true))
		.catch(e => end(66 + e));
})();