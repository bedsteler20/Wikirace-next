import path from "path";
import { exec } from "child_process";
import * as puppeteer from "puppeteer";

const srcDir = "src";

class PuppetUser {
    /**
     * @param {puppeteer.Browser} browser
     */
    constructor(browser) {
        this.browser = browser;
    }

    async start() {
        this.page = await this.browser.newPage();
        await this.page.goto("http://localhost:5230");
    }

    async inputField(selector, value) {
        await this.page.waitForSelector(selector);
        await this.page.evaluate(
            (selector, value) => {
                const input = document.querySelector(selector);
                input.value = value;
            },
            selector,
            value
        );
    }

    async click(selector) {
        await this.page.waitForSelector(selector);
        await this.page.click(selector);
    }

    async getInnerText(selector) {
        await this.page.waitForSelector(selector);
        const innerText = await this.page.evaluate((selector) => {
            return document.querySelector(selector).innerText;
        }, selector);

        return innerText;
    }

    async hasElement(selector) {
        return await this.page.evaluate((selector) => {
            return !!document.querySelector(selector);
        }, selector);
    }
}

class GameSession extends PuppetUser {
    /**
     * @param {puppeteer.Browser} browser
     * @param {string} name
     */
    constructor(browser, name) {
        super(browser);
        this.name = name;
    }

    async createGame() {
        await this.click("a[href='/session/create']");
        await this.inputField("#start-page", "Google");
        await this.inputField("#end-page", "Microsoft");
        await this.inputField("#DisplayName", this.name);
        await this.click("button[type='submit']");

        return await this.getInnerText("#game-id");
    }

    async joinGame(gameId) {
        await this.page.waitForSelector("a[href='/session/join']");
        await this.page.click("a[href='/session/join']");
        await this.inputField("#JoinCode", gameId);
        await this.inputField("#DisplayName", this.name);
        await this.click("button[type='submit']");
    }

    async hasJoined() {
        await this.page.waitForSelector(".lobby-player");
        return await this.page.evaluate((name) => {
            const players = document.querySelectorAll(".lobby-player");

            for (const player of players) {
                if (player.innerText.includes(name)) {
                    return true;
                }
            }

            return false;
        }, this.name);
    }

    async bodyIncludes(text) {
        return await this.page.evaluate((text) => {
            return document.body.innerText.includes(text);
        }, text);
    }

    async clickInGame(selector) {}
}

function assert(condition, message) {
    if (!condition) {
        throw new Error(message);
    } else {
        console.log("Passed assertion: ", message || "No message");
    }
}

function assertNot(condition, message) {
    if (condition) {
        throw new Error(message);
    } else {
        console.log("Passed assertion: ", message || "No message");
    }
}

function sleep(ms) {
    return new Promise((resolve) => setTimeout(resolve, ms));
}

async function main() {
    // exec("dotnet run", { cwd: srcDir });

    const player1 = new GameSession(
        await puppeteer.launch({
            headless: false,
        }),
        "Player 1"
    );
    const player2 = new GameSession(
        await puppeteer.launch({
            headless: false,
        }),
        "Player 2"
    );

    await player1.start();
    await player2.start();
    const gameId = await player1.createGame();
    await player2.joinGame(gameId);

    await sleep(4000);

    assert(await player1.hasJoined(), "Player 1 has not joined the game");
    assert(await player2.hasJoined(), "Player 2 has not joined the game");

    assertNot(
        await player2.hasElement("#StartGame"),
        "Player 2 should not have the start game button"
    );

    await player1.click("#StartGame");

    await player2.clickInGame("a[href='./California']");
    // Hopefully the this doesn't change in the future its checking 
    // for the longitude and latitude of the California page
    await sleep(2000);
    assert(await player2.bodyIncludes("114°8′ W to 124°26′ W"), "Player 2 should see the California page");
}

main();
